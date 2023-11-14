using Microsoft.Win32;
using PIIIProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameSummary gameSummary;
        private Quiz list = new Quiz();
        private string loadLocation;
        private string saveLocation;
        private int indexCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            Seed();
            LoadOneQuestion(indexCounter);
            MessageBox.Show(Instructions(), "WELCOME TO OUR QUIZ TRIVIA GAME!!!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Sets the default Questions
        /// </summary>
        private void Seed()
        {
            list.AddQuestion(new Question("Who was the first woman to win a Nobel Prize (in 1903)?",
                new string[] { "Andrea M. Ghez", "Maria Mayer", "Marie Curie", "Emmanuelle Charpentier" }, "Marie Curie"));
            list.AddQuestion(new Question("How long is an Olympic swimming pool (in meters)?",
                new string[] { "50m", "45m", "55m", "60m" }, "50m"));
            list.AddQuestion(new Question("Which country consumes the most chocolate per capita?", 
                new string[] { "Finland", "Belgium", "Norway", "Switzerland" }, "Switzerland"));
            list.AddQuestion(new Question("What is the most visited tourist attraction in the world?", 
                new string[] { "Eiffel Tower", "Colosseum", "Statue of Liberty", "Great Wall of China" }, "Eiffel Tower"));
            list.AddQuestion(new Question("Which fast food restaurant has the largest number of retail locations in the world?", 
                new string[] { "McDonald's", "Jack In The Box", "Subway", "Chipotle" }, "Subway"));
        }

        /// <summary>
        /// Handles the click event for the load button.
        /// Sets up the Quiz objects with a list of questions.
        /// Loads the first question on the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files | *.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                loadLocation = openFileDialog.FileName;
                if (!string.IsNullOrEmpty(loadLocation))
                {
                    list.QuestionsList.Clear();
                    list.GetQuestionsFromCSVFile(loadLocation);
                    LoadOneQuestion(indexCounter);
                }
            }

        }

        /// <summary>
        /// Clears the previous question and sets up the current one
        /// </summary>
        /// <param name="index"></param>
        private void LoadOneQuestion(int index)
        {
            ClearQuestion();

            txbQuestion.Text = list.QuestionsList[indexCounter].QuestionToAsk;
            IEnumerable<RadioButton> rdbChoices = stkChoices.Children.OfType<RadioButton>();

            for(int i = 0; i < list.QuestionsList[indexCounter].ArrayOfChoices.Length; i++)
            {
                rdbChoices.ElementAt(i).Content = list.QuestionsList[indexCounter].ArrayOfChoices[i];
            }

            txbQuestionNum.Text = list.GetQuestionNumber;

        }

        /// <summary>
        /// Clears the questions and multiple choice contents
        /// </summary>
        private void ClearQuestion()
        {
            txbQuestion.Text = string.Empty;
            IEnumerable<RadioButton> rdbChoices = stkChoices.Children.OfType<RadioButton>();

            foreach (RadioButton rdb in rdbChoices)
            {
                rdb.Content = string.Empty;
                rdb.IsChecked = false;
            }
        }

        /// <summary>
        /// Handles the click event of the submit button.
        /// Checks if the answer is correct. 
        /// Enables the Next button to give the player a choice of continuing or finishing the quiz.
        /// It it's the last question, show the game summary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string userAns = string.Empty;
            IEnumerable<RadioButton> rdbChoices = stkChoices.Children.OfType<RadioButton>();

            foreach (RadioButton rdb in rdbChoices)
            {
                if (rdb.IsChecked == true)
                {
                    userAns = rdb.Content as string;
                    list.AddAnswer(userAns);
                    break; 
                }
            }

            if (string.IsNullOrEmpty(userAns))
            {
                MessageBox.Show("Please select an answer.", "No answer chosen", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (userAns == list.QuestionsList[indexCounter].CorrectAnswer)
            {
                MessageBox.Show($"Your answer is correct.", "Answer", MessageBoxButton.OK, MessageBoxImage.Information );
            }
            else
            {
                MessageBox.Show($"Your answer is incorrect. The answer is {list.QuestionsList[indexCounter].CorrectAnswer}.", "Answer", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            btnSubmit.IsEnabled = false;
            btnNext.IsEnabled = true;

            if(indexCounter == list.QuestionsList.Count - 1)
            {
                gameSummary = new GameSummary(list, list.CreateGameSummaryText());
                gameSummary.Show();
                ResetQuiz();
            }
        }

        /// <summary>
        /// Handles the click event for the Next button.
        /// Goes to next question and displays it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            indexCounter++;
            LoadOneQuestion(indexCounter);
            btnNext.IsEnabled = false;
            btnSubmit.IsEnabled = true;
        }

        /// <summary>
        /// Handles the click event of the Finish button.
        /// Calls the Game Summary Window to show the summary.
        /// Resets the quiz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            gameSummary = new GameSummary(list, list.CreateGameSummaryText());
            gameSummary.Show();
            ResetQuiz();
        }

        /// <summary>
        /// Ends the quiz by resetting the score and answers.
        /// Loads the first question again.
        /// </summary>
        private void ResetQuiz()
        {
            list.EndQuiz();
            btnSubmit.IsEnabled = true;
            indexCounter = 0;
            LoadOneQuestion(indexCounter);
        }

        /// <summary>
        /// Handles the click event on the menu item About
        /// Shows the instructions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Instructions(), "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Handles the click event on the Save button.
        /// Creates a .txt file with the game summary that the player can save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "TXT File | *.txt";
            if (save.ShowDialog() == true)
            {
                saveLocation = save.FileName;

            }

            if (!string.IsNullOrEmpty(saveLocation))
            {
                File.WriteAllText(saveLocation, list.CreateGameSummaryText());
                Process.Start(new ProcessStartInfo
                {
                    FileName = Path.GetFullPath(saveLocation),
                    UseShellExecute = true
                });
            }

        }

        /// <summary>
        /// Creates the string with instructions
        /// </summary>
        /// <returns>String of instructions</returns>
        private string Instructions()
        {             
            return $"How to play: \n" +
                   $"Read the question and select one of the choices as your answer. " +
                   $"Then, click the Submit button. If you want to continue the quiz, click the Next button. " +
                   $"However, if you want to finish it immediately, click the Finish button. All unanswered question will be set as incorrect. " +
                   $"You can save the game summary as a .txt file when the quiz finishes." +
                   $"\n\nHow to load your own quiz: \n" +
                   $"You can load your own quiz by clicking the Load button. The app accepts a .csv file. The first column should contain the question, the next four, the multiple choice " +
                   $"and the last one, the correct answer.";
        }
    }
}
