using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace RockPaperScissors
{
    public partial class MainPage : ContentPage
    {
        int userWins = 0;
        int computerWins = 0;
        int ties = 0;
        int roundsPlayed = 0;
        string message;

        public MainPage()
        {
            InitializeComponent();
        }

        void RockButton_Clicked(object sender, System.EventArgs e)
        {
            PlayGame("rock");
        }

        void ScissorsButton_Clicked(object sender, System.EventArgs e)
        {
            PlayGame("scissors");
        }

        void PaperButton_Clicked(object sender, System.EventArgs e)
        {
            PlayGame("paper");
        }

        void PlayGame(string userChoice)
        {
            string computerChoice = GetComputerChoice();
            DetermineWinner(userChoice, computerChoice);
            UpdateScores();
            DisplayResult(userChoice, computerChoice);
            CheckForGameOver();
        }

        string GetComputerChoice()
        {
            Random rand = new Random();
            int choice = rand.Next(1, 4);
            switch (choice)
            {
                case 1: return "rock";
                case 2: return "paper";
                case 3: return "scissors";
                default: return "";
            }
        }

        void DetermineWinner(string userChoice, string computerChoice)
        {
            if (userChoice == computerChoice)
            {
                ties++;
            }
            else if ((userChoice == "rock" && computerChoice == "scissors") ||
                     (userChoice == "paper" && computerChoice == "rock") ||
                     (userChoice == "scissors" && computerChoice == "paper"))
            {
                userWins++;
            }
            else
            {
                computerWins++;
            }
            roundsPlayed = userWins + computerWins + ties;
        }

        void UpdateScores()
        {
            userScoreLabel.Text = userWins.ToString();
            computerScoreLabel.Text = computerWins.ToString();
            drawScoreLabel.Text = ties.ToString();
            roundsLabel.Text = roundsPlayed.ToString();
        }

        void EndScores()
        {
            userScoreLabel.Text = Convert.ToString(userWins - userWins);
            computerScoreLabel.Text = Convert.ToString(computerWins - computerWins);
            drawScoreLabel.Text = Convert.ToString(ties - ties);
            roundsLabel.Text = Convert.ToString(roundsPlayed - roundsPlayed);
        }

        void DisplayResult(string userChoice, string computerChoice)
        {
            string result = $"You chose {userChoice}, computer chose {computerChoice}. ";
            if ((userChoice == "rock" && computerChoice == "scissors") ||
                (userChoice == "paper" && computerChoice == "rock") ||
                (userChoice == "scissors" && computerChoice == "paper"))
            {
                result += $"You win! {GetGameOverMessage()}";
            }
            else if (userChoice == computerChoice)
            {
                result += $"It's a tie! {GetGameOverMessage()}";
            }
            else
            {
                result += $"Computer wins! {GetGameOverMessage()}";
            }
            resultLabel.Text = result;
        }

        void CheckForGameOver()
        {
            if ((userWins >= 3 || computerWins >= 3) && (Math.Abs(userWins - computerWins) >= 1))
            {
                GameOver();
            }
            else if ((roundsPlayed % 6 == 0) && GetGameOverMessage() == "The game is inconclusive.")
            {
                AskToContinue();
            }
        }

        void GameOver()
        {
            string gameOverMessage = GetGameOverMessage();
            DisplayAlert("Game Over", gameOverMessage, "OK").ContinueWith(t =>
            {
                if ((userWins >= 3 || computerWins >= 3) && (Math.Abs(userWins - computerWins) >= 1))
                {
                    // End the game and reset scores
                    EndScores();
                }
                else
                {
                    // Update scores
                    UpdateScores();
                }
            });
        }

        string GetGameOverMessage()
        {
            if (userWins > computerWins)
            {
                return "You are the overall winner!";
            }
            else if (computerWins > userWins)
            {
                return "Computer is the overall winner!";
            }
            else
            {
                return "The game is inconclusive.";
            }
        }

        void AskToContinue()
        {
            DisplayAlert("Continue?", "Do you want to continue the game?", "Yes", "No").ContinueWith(t =>
            {
                if (((Task<bool>)t).Result)
                {
                    // Continue the game
                    UpdateScores();
                }
                else
                {
                    // End the game and reset scores
                    GameOver();
                }
            });
        }
    }
}