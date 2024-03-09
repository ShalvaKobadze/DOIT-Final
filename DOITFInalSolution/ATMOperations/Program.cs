using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome, we are at your service");

        int loginAttempts = 0;
        bool loggedIn = false;

        while (loginAttempts < 4 && !loggedIn)
        {
            Console.WriteLine("Do you want to create a new account or log in to an existing one?");
            Console.WriteLine("1. Create a new account");
            Console.WriteLine("2. Log in to an existing account");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (CreateAccount())
                    {
                        Console.WriteLine("Account created successfully.");
                        loggedIn = true;
                    }
                    break;
                case "2":
                    if (Login())
                    {
                        Console.WriteLine("Logged in successfully.");
                        loggedIn = true;
                    }
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            if (!loggedIn)
            {
                loginAttempts++;
                Console.WriteLine("Invalid data, please try again.");
            }
        }

        if (!loggedIn)
        {
            Console.WriteLine("Exceeded maximum login attempts. Please try again later.");
            return;
        }

        // At this point, the user is logged in and can proceed with ATM operations
        // You can implement the ATM operations here
    }

    static bool CreateAccount()
    {
        Console.WriteLine("Creating a new account...");

        // Get user input for username
        Console.Write("Enter your username (max 35 characters): ");
        string username = Console.ReadLine();
        if (string.IsNullOrEmpty(username) || username.Length > 35)
        {
            Console.WriteLine("Invalid data.");
            return false;
        }

        // Get user input for ID
        Console.Write("Enter your ID (11-digit number): ");
        string idInput = Console.ReadLine();
        if (!long.TryParse(idInput, out long id) || idInput.Length != 11)
        {
            Console.WriteLine("Invalid data.");
            return false;
        }

        // Get user input for password
        Console.Write("Enter your password (4-digit number): ");
        string passwordInput = Console.ReadLine();
        if (!int.TryParse(passwordInput, out int password) || passwordInput.Length != 4)
        {
            Console.WriteLine("Invalid data.");
            return false;
        }

        // Create a new account object
        Account account = new Account(username, id, password);

        // Serialize the account object to JSON and write it to a file
        string json = JsonConvert.SerializeObject(account);
        File.WriteAllText($"Users/{username}.json", json);

        return true;
    }

    static bool Login()
    {
        Console.WriteLine("Logging in to an existing account...");

        // Get user input for username
        Console.Write("Enter your username: ");
        string username = Console.ReadLine();

        // Check if the user's JSON file exists
        string filePath = $"Users/{username}.json";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Invalid data.");
            return false;
        }

        // Read the user's data from the JSON file
        string json = File.ReadAllText(filePath);
        Account existingAccount = JsonConvert.DeserializeObject<Account>(json);

        // Get user input for password
        Console.Write("Enter your password: ");
        string passwordInput = Console.ReadLine();
        if (existingAccount.Password != passwordInput)
        {
            Console.WriteLine("Invalid data.");
            return false;
        }

        return true;
    }
}

class Account
{
    public string Username { get; set; }
    public long ID { get; set; }
    public string Password { get; set; }

    public Account(string username, long id, int password)
    {
        Username = username;
        ID = id;
        Password = password.ToString("D4");
    }
}