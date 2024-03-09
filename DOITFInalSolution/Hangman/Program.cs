namespace Hangman
{
    class HangmanGame
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Player");
            Console.WriteLine("Welcome to Hangman!");

            // List of words for the game
            string[] words = { "organize", "absorb", "sue", "tighten", "reckon", "support", "pretend", "hand", "cook", "knock" };

            // Select a random word from the list
            Random random = new Random();
            string selectedWord = words[random.Next(words.Length)];

            // Convert the selected word to char array for tracking guessed letters
            char[] wordToGuess = new char[selectedWord.Length];
            for (int i = 0; i < selectedWord.Length; i++)
            {
                wordToGuess[i] = '_';
            }

            int attempts = 6;
            bool wordGuessed = false;

            while (attempts > 0 && !wordGuessed)
            {
                Console.WriteLine();
                Console.WriteLine("Word to guess: " + string.Join(" ", wordToGuess));
                Console.WriteLine("Attempts left: " + attempts);
                Console.WriteLine("Enter your guess or type 'exit' to quit:");

                string guess = Console.ReadLine().ToLower();

                if (guess == "exit")
                {
                    Console.WriteLine("Exiting the game.");
                    break;
                }
                else if (guess.Length == 1 && char.IsLetter(guess[0]))
                {
                    bool found = false;
                    for (int i = 0; i < selectedWord.Length; i++)
                    {
                        if (selectedWord[i] == guess[0])
                        {
                            wordToGuess[i] = guess[0];
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        attempts--;
                        Console.WriteLine("Letter not found. Attempts left: " + attempts);
                    }
                }
                else if (guess.Length == selectedWord.Length && guess == selectedWord)
                {
                    wordGuessed = true;
                }
                else
                {
                    Console.WriteLine("Invalid value, try again.");
                }

                if (new string(wordToGuess) == selectedWord)
                {
                    wordGuessed = true;
                }
            }

            if (wordGuessed)
            {
                Console.WriteLine("Congratulations, you won the game!");
            }
            else
            {
                Console.WriteLine("Game lost, you are dead!");
            }
        }
    }
}
