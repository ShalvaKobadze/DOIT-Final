namespace GuessTheNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome Player.");

            while (true)
            {
                Console.WriteLine("Select game mode:");
                Console.WriteLine("1. Easy Mode (Numbers from 1 to 25)");
                Console.WriteLine("2. Medium Mode (Numbers from 1 to 50)");
                Console.WriteLine("3. Hard Mode (Numbers from 1 to 100)");
                Console.WriteLine("4. Exit");

                Console.Write("Enter choice (1/2/3/4): ");
                string choice = Console.ReadLine();

                if (!IsValidChoice(choice))
                {
                    Console.WriteLine("Invalid Value");
                    continue;
                }

                if (choice == "4")
                {
                    Console.WriteLine("Exiting the game.");
                    break;
                }

                int maxNumber = choice == "1" ? 25 : choice == "2" ? 50 : 100;
                int targetNumber = new Random().Next(1, maxNumber + 1);
                int attempts = 10;

                Console.WriteLine($"You have {attempts} attempts to guess the number.");

                while (attempts > 0)
                {
                    Console.Write("Enter your guess: ");
                    if (!int.TryParse(Console.ReadLine(), out int guess))
                    {
                        Console.WriteLine("Invalid Value");
                        continue;
                    }

                    if (guess == targetNumber)
                    {
                        Console.WriteLine("Congratulations! You guessed the number.");
                        break;
                    }
                    else if (guess < targetNumber)
                    {
                        Console.WriteLine("Number value is higher");
                    }
                    else
                    {
                        Console.WriteLine("Number value is lower");
                    }

                    attempts--;
                    Console.WriteLine($"You have {attempts} attempts left.");
                }

                if (attempts == 0)
                {
                    Console.WriteLine($"Sorry, you've run out of attempts. The correct number was {targetNumber}.");
                }
            }
        }

        static bool IsValidChoice(string choice)
        {
            return choice == "1" || choice == "2" || choice == "3" || choice == "4";
        }
    }
}
