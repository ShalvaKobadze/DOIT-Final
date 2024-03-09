namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello user. Welcome to Calculator");

            while (true)
            {
                Console.WriteLine("Select operation:");
                Console.WriteLine("1. Addition");
                Console.WriteLine("2. Subtraction");
                Console.WriteLine("3. Multiplication");
                Console.WriteLine("4. Division");
                Console.WriteLine("5. Exit");

                Console.Write("Enter choice (1/2/3/4/5): ");
                string choice = Console.ReadLine();

                if (!IsValidChoice(choice))
                {
                    Console.WriteLine("Invalid Value");
                    continue;
                }

                if (choice == "5")
                {
                    Console.WriteLine("Exiting the calculator.");
                    break;
                }

                Console.Write("Enter first number: ");
                if (!double.TryParse(Console.ReadLine(), out double num1))
                {
                    Console.WriteLine("Invalid Value");
                    continue;
                }

                Console.Write("Enter second number: ");
                if (!double.TryParse(Console.ReadLine(), out double num2))
                {
                    Console.WriteLine("Invalid Value");
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Result: {num1 + num2}");
                        break;
                    case "2":
                        Console.WriteLine($"Result: {num1 - num2}");
                        break;
                    case "3":
                        Console.WriteLine($"Result: {num1 * num2}");
                        break;
                    case "4":
                        if (num2 == 0)
                            Console.WriteLine("Error! Division by zero.");
                        else
                            Console.WriteLine($"Result: {num1 / num2}");
                        break;
                }
            }
        }

        static bool IsValidChoice(string choice)
        {
            return choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5";
        }
    }
}
