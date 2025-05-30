// CET137 Assessment 1 - Scenario 1: Number Game
// Author: Anish Pangeni
// Description: Console-based game where player and computer get two cards (1-8).
// Goal: Get the lowest score (difference between cards). Played over 3 rounds.
// Player can swap one card per round. Computer swaps until score < 3.
// Logs actions to a file and finds winner by score.

using System; // for console
using System.IO; // for file logging

namespace NumberGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(); // create game
            game.Start(); // run game
        }
    }

    class Game
    {
        private Player? player;
        private Player? computer;
        private StreamWriter logWriter; // for logging to gamelog.txt

        public Game()
        {
            logWriter = new StreamWriter("gamelog.txt", true); // append logs
        }

        public void Start()
        {
            logWriter.WriteLine($"\n========== Game Started at {DateTime.Now} ==========");

            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? "Player";
            player = new Player(name);
            computer = new Player("Computer");
            logWriter.WriteLine($"Player Name: {name}\n");

            for (int round = 1; round <= 3; round++)
            {
                Console.WriteLine($"\n--- Round {round} ---");
                logWriter.WriteLine($"\n--- Round {round} ---");

                player!.DealCards();
                computer!.DealCards();

                player.ShowCards(); // show player's cards
                logWriter.WriteLine($"{player.Name}'s cards: {player.Card1}, {player.Card2}");

                Console.Write("Swap a card? (yes/no): ");
                string? input = Console.ReadLine();
                string choice = (input ?? "").ToLower();
                if (choice == "yes")
                {
                    Console.Write("Swap card 1 or 2? ");
                    string? cardInput = Console.ReadLine();
                    int cardChoice = 1;
                    if (!string.IsNullOrEmpty(cardInput) && int.TryParse(cardInput, out int parsedChoice) && (parsedChoice == 1 || parsedChoice == 2))
                    {
                        cardChoice = parsedChoice;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Using card 1.");
                    }
                    player.SwapCard(cardChoice); // player swaps card
                    Console.WriteLine($"Your cards: {player.Card1}, {player.Card2}");
                    logWriter.WriteLine($"{player.Name} swapped card {cardChoice}. Cards: {player.Card1}, {player.Card2}");
                }

                while (computer.GetScore() >= 3)
                {
                    computer.SwapCard(new Random().Next(1, 3)); // computer swaps until score < 3
                }
                logWriter.WriteLine($"{computer.Name}'s cards (hidden): {computer.Card1}, {computer.Card2}");

                player.AddToTotalScore();
                computer.AddToTotalScore();
            }

            // show final scores
            logWriter.WriteLine($"\nFinal Scores: {player.Name}: {player.TotalScore}, {computer.Name}: {computer.TotalScore}");
            Console.WriteLine("\n--- Game Over ---");
            Console.WriteLine($"Your score: {player.TotalScore}");
            Console.WriteLine($"Computer score: {computer.TotalScore}");

            string winner = DetermineWinner();
            Console.WriteLine($"Winner: {winner}");
            logWriter.WriteLine($"Winner: {winner}");

            logWriter.Close(); // close log file
        }

        private string DetermineWinner()
        {
            // compare scores
            if (player!.TotalScore < computer!.TotalScore) return player.Name;
            else if (player.TotalScore > computer.TotalScore) return computer.Name;
            else
            {
                // tiebreaker: compare card totals
                int playerSum = player.Card1 + player.Card2;
                int computerSum = computer.Card1 + computer.Card2;
                return playerSum < computerSum ? player.Name : computer.Name;
            }
        }
    }

    class Player
    {
        public string Name { get; set; }
        public int Card1 { get; set; } // first card
        public int Card2 { get; set; } // second card
        public int TotalScore { get; private set; } // total score

        public Player(string name)
        {
            Name = name;
            TotalScore = 0;
        }

        public void DealCards()
        {
            Random rand = new Random();
            Card1 = rand.Next(1, 9); // random card 1-8
            Card2 = rand.Next(1, 9);
        }

        public void ShowCards()
        {
            Console.WriteLine($"Your cards: {Card1}, {Card2}");
        }

        public void SwapCard(int cardNumber)
        {
            Random rand = new Random();
            if (cardNumber == 1) Card1 = rand.Next(1, 9);
            else Card2 = rand.Next(1, 9);
        }

        public int GetScore()
        {
            return Math.Abs(Card1 - Card2); // score is card difference
        }

        public void AddToTotalScore()
        {
            TotalScore += GetScore(); // add round score
        }
    }
}