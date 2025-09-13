using System;

class GuessMyNumber
{
    static void Main()
    {

        {
            bool playAgain = true;
            Random random = new Random();


            while (playAgain)
            {
                // Generate random magic number between 1 and 100
                int magicNumber = random.Next(1, 101);
                int guess = 0;
                int guessCount = 0;

                Console.WriteLine("I am thinking of a number between 1 and 100.");

                // Loop until the user guesses the magic number
                while (guess != magicNumber)
                {
                    Console.Write("What is your guess? ");
                    if (!int.TryParse(Console.ReadLine(), out guess))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        continue;
                    }

                    guessCount++;

                    if (guess < magicNumber)
                        Console.WriteLine("Higher");
                    else if (guess > magicNumber)
                        Console.WriteLine("Lower");
                    else
                        Console.WriteLine("You guessed it right!");
                }

                // Inform number of guesses
                Console.WriteLine($"You guessed the number in {guessCount} guesses.");

                // Ask if the user wants to play again
                Console.Write("Do you want to play again? (yes/no): ");
                string response = Console.ReadLine().Trim().ToLower();

                playAgain = (response == "yes");
            }

            Console.WriteLine("Thanks for playing! Have a bless day. Goodbye for now.");
        }
    }

}


