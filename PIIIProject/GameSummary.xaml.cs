using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using Path = System.IO.Path;
using PIIIProject.Models;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for GameSummary.xaml
    /// </summary>
    public partial class GameSummary : Window
    {
        private Quiz gameSummary;
        private string previousSummaryText;
        private string saveLocation;

        public GameSummary(Quiz summary, string summaryText)
        {
            InitializeComponent();
            gameSummary = summary;
            previousSummaryText = summaryText;
            //Displays the summary on screen 
            lsbSummary.ItemsSource = gameSummary.QuestionsList;
            txbScore.Text = gameSummary.Score.ToString() + " / " + gameSummary.QuestionsList.Count.ToString();
        }

        /// <summary>
        /// Handles the click event on the Save button.
        /// Creates a .txt file with the game summary that the player can save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "TXT File | *.txt";
            if (save.ShowDialog() == true)
            {
                saveLocation = save.FileName;
            }

            if (!string.IsNullOrEmpty(saveLocation))
            {
                File.WriteAllText(saveLocation, previousSummaryText);
                Process.Start(new ProcessStartInfo
                {
                    FileName = Path.GetFullPath(saveLocation),
                    UseShellExecute = true
                });
            }
        }

        /// <summary>
        /// Handles the click event on the Exit button.
        /// Closes the Game Summary window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the click event on the menu item About.
        /// Displays the instructions on how to save the player's results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Save your results by clicking the Save button. It creates a .txt file and will ask for a save location.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
