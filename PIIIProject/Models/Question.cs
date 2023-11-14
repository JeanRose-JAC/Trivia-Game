using System;
using System.Collections.Generic;
using System.Text;

namespace PIIIProject.Models
{
    /// <summary>
    /// Creates an object for holding information regarding a quiz question
    /// </summary>
    public class Question
    {
        private const int NUM_CHOICES = 4;
        private string _questionToAsk;
        private string _correctAnswer;
        private string _playerAnswer;
        private string[] _arrayOfChoices = new string[NUM_CHOICES];

        /// <summary>
        /// Sets the QuestionToAsk, CorrectAnswer and ArrayOfChoices according to the input
        /// Sets the PlayerAnswer as NA by default.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="choices"></param>
        /// <param name="correctAnswer"></param>
        public Question(string question, string[] choices, string correctAnswer)
        {
            QuestionToAsk = question;
            CorrectAnswer = correctAnswer;
            ArrayOfChoices = choices;
            PlayerAnswer = "NA";
        }

        /// <summary>
        /// Gets and sets the quiz question
        /// </summary>
        public string QuestionToAsk
        {
            get { return _questionToAsk; }
            set { _questionToAsk = value; }
        }

        /// <summary>
        /// Gets and sets the correct answer for the question
        /// </summary>
        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set { _correctAnswer = value; }
        }

        /// <summary>
        /// Gets and sets the player answer for the question
        /// </summary>
        public string PlayerAnswer
        {
            get { return _playerAnswer; }
            set { _playerAnswer = value; }
        }


        /// <summary>
        /// Gets and sets the multiple choice for the question
        /// </summary>
        public string[] ArrayOfChoices
        {
            get { return _arrayOfChoices; }
            set { _arrayOfChoices = value; }
        }

        /// <summary>
        /// Resets the PlayerAnswer to NA for when the game is done
        /// </summary>
        public void ResetPlayerAnswer()
        {
            PlayerAnswer = "NA";
        }

        /// <summary>
        /// Sets up the string to return in order to get the question,
        /// player's answer and correct answer.
        /// </summary>
        /// <returns>String containing the question, player's answer and correct answer.</returns>
        public string GetQuestionAndAnswer()
        {
            return string.Format($"Question: {this.QuestionToAsk}\n" +
                                 $"Your Answer: {this.PlayerAnswer}\n" +
                                 $"Correct Answer: {this.CorrectAnswer}\n\n");
        }

        /// <summary>
        /// Sets up the string containing all the properties of a Question
        /// </summary>
        /// <returns>String with all of the properties of a Question</returns>
        public override string ToString()
        {
            string choices = string.Empty;

            foreach(string choice in ArrayOfChoices)
                choices += choice + ' ';

            return string.Format($"Question: {QuestionToAsk}\n" +
                                 $"Choices: {choices}\n" +
                                 $"Correct Answer: {CorrectAnswer}\n" +
                                 $"Player Answeer: {PlayerAnswer}");
        }
    }
}
