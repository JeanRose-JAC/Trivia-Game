using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Documents;

namespace PIIIProject.Models
{
    /// <summary>
    /// Creates an object to hold a list of Questions
    /// </summary>
    public class Quiz
    {
        private List<Question> _questionsList;
        private int _score = 0;
        private int _questionNumber = 0;

        /// <summary>
        /// Initializes the QuestionList
        /// </summary>
        public Quiz()
        {
            QuestionsList = new List<Question>();
        }

        /// <summary>
        /// Gets and sets the list of Questions
        /// </summary>
        public List<Question> QuestionsList
        {
            get { return _questionsList; }
            set { _questionsList = value; } 
        }

        /// <summary>
        /// Gets and sets the score
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public int Score
        {
            get { return _score; }
            private set
            {
                if (value < 0)
                    throw new ArgumentException("score", "Score must be positive.");

                _score = value;
            }
        }

        /// <summary>
        /// Returns the string containing the question number is out of the total
        /// </summary>
        public string GetQuestionNumber
        {
            get { return (_questionNumber + 1).ToString() + " / " + QuestionsList.Count.ToString(); }
        }

        /// <summary>
        /// Adds a Question to the list
        /// </summary>
        /// <param name="ques"></param>
        public void AddQuestion(Question ques)
        {
            _questionsList.Add(ques);
        }

        /// <summary>
        /// Creates the Question object using the arguments passed
        /// Then adds it to the list
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="choices"></param>
        /// <param name="correctAns"></param>
        public void AddQuestion(string ques, string[] choices, string correctAns)
        {
            _questionsList.Add(new Question(ques, choices, correctAns));
        }

        /// <summary>
        /// Gets a list of question from a CSV file
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Question> GetQuestionsFromCSVFile(string location)
        {
            StreamReader sr = null;
            string line;
            string[] seperatedValues;
            const int LENGTH = 6;

            if (File.Exists(location))
            {
                using (sr = new StreamReader(location))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        seperatedValues = line.Split(',');

                        if (seperatedValues.Length == LENGTH)
                            this.AddQuestion(seperatedValues[0],
                            new string[] { seperatedValues[1], seperatedValues[2], seperatedValues[3], seperatedValues[4] },
                            seperatedValues[5]);
                        else
                            throw new ArgumentException("Incorrect file content.");
                    }
                }
            }

            return _questionsList;
        }

        /// <summary>
        /// Adds the player's answer to the Question.
        /// If it's correct, a point is added to score
        /// </summary>
        /// <param name="ans"></param>
        public void AddAnswer(string ans)
        {
            this.QuestionsList[_questionNumber].PlayerAnswer = ans;
            if (this.QuestionsList[_questionNumber].PlayerAnswer == this.QuestionsList[_questionNumber].CorrectAnswer)
            {
                this.AddScore();
            }
            _questionNumber++;
        }

        /// <summary>
        /// Adds score if the answer is correct
        /// </summary>
        private void AddScore()
        {
            this.Score += 1;
        }

        /// <summary>
        /// Resets the score and question number tracker to 0
        /// </summary>
        public void EndQuiz()
        {
            this.Score = 0;
            this._questionNumber = 0;

            foreach(Question question in this.QuestionsList)
            {
                question.ResetPlayerAnswer();
            }
        }


        /// <summary>
        /// Creates a string containing the summary of the game.
        /// For each question, the question, player answer and correct answer are added.
        /// At the end, the score is displayed
        /// </summary>
        /// <returns>String with the game summary text</returns>
        public string CreateGameSummaryText()
        {
            StringBuilder builder = new StringBuilder();

            foreach(Question question in this.QuestionsList)
            {
                builder.AppendLine(question.GetQuestionAndAnswer());
            }

            builder.AppendLine($"Score: {this.Score} / {this.QuestionsList.Count}");

            return builder.ToString();
        }

    }
}
