using System;

using System.IO;

using System.Linq;

using System.Text.Json;

namespace ATMConsoleApp

{

    class ClientInfo

    {

        public string UserId { get; set; }

        public string Username { get; set; }

        public int Pin { get; set; }

    }

    class BalanceInfo

    {

        public string Username { get; set; }

        public decimal Balance { get; set; }

    }

    class Program

    {

        static string userInfoFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "UserInfo.json");

        static string balanceInfoFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "BalanceInfo.json");

        static string currentUsername = null;

        static void Main(string[] args)

        {

            while (true)

            {

                Console.WriteLine("Welcome! We are at your service");

                if (currentUsername == null)

                {

                    Console.WriteLine("1. Would you like to create new account");

                    Console.WriteLine("2. Here you can access existing account");

                    Console.WriteLine("3. Exit");

                }

                else

                {

                    Console.WriteLine("1. Withdraw");

                    Console.WriteLine("2. Deposit");

                    Console.WriteLine("3. Show Current Balance");

                    Console.WriteLine("4. Exit");

                }

                Console.Write("Please choose an option: ");

                int choice;

                if (!int.TryParse(Console.ReadLine(), out choice))

                {

                    Console.WriteLine("Invalid value. Please enter correct number.");

                    continue;

                }

                if (currentUsername == null)

                {

                    switch (choice)

                    {

                        case 1:

                            CreateNewAccount();

                            break;

                        case 2:

                            AccessExistingAccount();

                            break;

                        case 3:

                            Exit();

                            return;

                        default:

                            Console.WriteLine("Invalid value. Please enter correct number.");

                            break;

                    }

                }

                else

                {

                    switch (choice)

                    {

                        case 1:

                            Withdraw();

                            break;

                        case 2:

                            Deposit();

                            break;

                        case 3:

                            ShowBalance();

                            break;

                        case 4:

                            Exit();

                            break;

                        default:

                            Console.WriteLine("Invalid value. Please enter correct number.");

                            break;

                    }

                }

            }

        }

        static void CreateNewAccount()

        {

            Console.WriteLine("\nCreating New Account...");

            Console.Write("Enter User ID (At least 11 digits): ");

            string userId = Console.ReadLine();

            if (userId.Length != 11 || !userId.All(char.IsDigit))

            {

                Console.WriteLine("User ID must be 11 digits.");

                return;

            }

            if (CheckUserIdExists(userId))

            {

                Console.WriteLine("User ID already exists. Please choose a different User ID.");

                return;

            }

            Console.Write("Enter Username: ");

            string username = Console.ReadLine();

            if (CheckUsernameExists(username))

            {

                Console.WriteLine("Username already exists. Please choose a different username.");

                return;

            }

            Console.Write("Enter PIN (in total 4 digits): ");

            int pin;

            if (!int.TryParse(Console.ReadLine(), out pin) || pin < 1000 || pin > 9999)

            {

                Console.WriteLine("PIN must be a 4-digit integer.");

                return;

            }

            Console.Write("Confirm PIN: ");

            int confirmPin;

            if (!int.TryParse(Console.ReadLine(), out confirmPin))

            {

                Console.WriteLine("Invalid PIN format.");

                return;

            }

            if (pin != confirmPin)

            {

                Console.WriteLine("PINs do not match. Account creation failed.");

                return;

            }

            var clientInfo = new ClientInfo { UserId = userId, Username = username, Pin = pin };

            string json = JsonSerializer.Serialize(clientInfo);

            using (StreamWriter sw = File.AppendText(userInfoFilePath))

            {

                sw.WriteLine(json);

            }

            using (StreamWriter sw = File.AppendText(balanceInfoFilePath))

            {

                sw.WriteLine(JsonSerializer.Serialize(new BalanceInfo { Username = username, Balance = 0 }));

            }

            Console.WriteLine("Account created successfully!");

        }

        static void AccessExistingAccount()

        {

            Console.WriteLine("\nAccessing Existing Account...");

            Console.Write("Please Enter Username: ");

            string username = Console.ReadLine();

            string clientInfoJson = GetClientInfo(username);

            if (clientInfoJson == null)

            {

                Console.WriteLine("Unfortunately this username does not exists.");

                return;

            }

            var clientInfo = JsonSerializer.Deserialize<ClientInfo>(clientInfoJson);

            Console.Write("Enter PIN: ");

            int pin;

            if (!int.TryParse(Console.ReadLine(), out pin))

            {

                Console.WriteLine("Invalid PIN format.");

                return;

            }

            if (pin != clientInfo.Pin)

            {

                Console.WriteLine("Invalid PIN.");

                return;

            }

            Console.WriteLine($"Logged in as {username}.");

            currentUsername = username;

        }

        static bool CheckUsernameExists(string username)

        {

            if (!File.Exists(userInfoFilePath))

                return false;

            string[] lines = File.ReadAllLines(userInfoFilePath);

            foreach (string line in lines)

            {

                var clientInfo = JsonSerializer.Deserialize<ClientInfo>(line);

                if (clientInfo.Username == username)

                    return true;

            }

            return false;

        }

        static bool CheckUserIdExists(string userId)

        {

            if (!File.Exists(userInfoFilePath))

                return false;

            string[] lines = File.ReadAllLines(userInfoFilePath);

            foreach (string line in lines)

            {

                var clientInfo = JsonSerializer.Deserialize<ClientInfo>(line);

                if (clientInfo.UserId == userId)

                    return true;

            }

            return false;

        }

        static string GetClientInfo(string username)

        {

            if (!File.Exists(userInfoFilePath))

                return null;

            string[] lines = File.ReadAllLines(userInfoFilePath);

            foreach (string line in lines)

            {

                var clientInfo = JsonSerializer.Deserialize<ClientInfo>(line);

                if (clientInfo.Username == username)

                    return line;

            }

            return null;

        }

        static void Withdraw()

        {

            Console.Write("Enter amount to withdraw: ");

            decimal amount;

            if (!decimal.TryParse(Console.ReadLine(), out amount))

            {

                Console.WriteLine("Invalid amount.");

                return;

            }

            string clientInfoJson = GetClientInfo(currentUsername);

            var clientInfo = JsonSerializer.Deserialize<ClientInfo>(clientInfoJson);

            string balanceInfoJson = GetBalanceInfo(currentUsername);

            var balanceInfo = JsonSerializer.Deserialize<BalanceInfo>(balanceInfoJson);

            if (amount > balanceInfo.Balance)

            {

                Console.WriteLine("Insufficient balance.");

                return;

            }

            balanceInfo.Balance -= amount;

            UpdateBalance(balanceInfo);

            Console.WriteLine($"Withdrawal successful. New balance: {balanceInfo.Balance}");

        }

        static void Deposit()

        {

            Console.Write("Enter amount to deposit: ");

            decimal amount;

            if (!decimal.TryParse(Console.ReadLine(), out amount))

            {

                Console.WriteLine("Invalid amount.");

                return;

            }

            string balanceInfoJson = GetBalanceInfo(currentUsername);

            var balanceInfo = JsonSerializer.Deserialize<BalanceInfo>(balanceInfoJson);

            balanceInfo.Balance += amount;

            UpdateBalance(balanceInfo);

            Console.WriteLine($"Deposit successful. New balance: {balanceInfo.Balance}");

        }

        static void ShowBalance()

        {

            string balanceInfoJson = GetBalanceInfo(currentUsername);

            var balanceInfo = JsonSerializer.Deserialize<BalanceInfo>(balanceInfoJson);

            Console.WriteLine($"Current Balance: {balanceInfo.Balance}");

        }

        static void Exit()

        {

            Console.WriteLine("Sorry to see you leave. Hope to see you soon!");

            Environment.Exit(0);

        }

        static void UpdateBalance(BalanceInfo updatedBalanceInfo)

        {

            string[] lines = File.ReadAllLines(balanceInfoFilePath);

            string updatedJson = JsonSerializer.Serialize(updatedBalanceInfo);

            for (int i = 0; i < lines.Length; i++)

            {

                var balanceInfo = JsonSerializer.Deserialize<BalanceInfo>(lines[i]);

                if (balanceInfo.Username == updatedBalanceInfo.Username)

                {

                    lines[i] = updatedJson;

                    break;

                }

            }

            File.WriteAllLines(balanceInfoFilePath, lines);

        }

        static string GetBalanceInfo(string username)

        {

            if (!File.Exists(balanceInfoFilePath))

                return null;

            string[] lines = File.ReadAllLines(balanceInfoFilePath);

            foreach (string line in lines)

            {

                var balanceInfo = JsonSerializer.Deserialize<BalanceInfo>(line);

                if (balanceInfo.Username == username)

                    return line;

            }

            return null;

        }

    }

}