// Quizzy.Models.Buisness_Layer.quiz.AttemptQuizBL.cs
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.DataLayer_Models;
using Quizzy.Models.Data_Layer.quiz;
using System;
using System.Data;
using System.Collections.Generic;

namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class AttemptQuizBL
    {
        // Get quiz questions for student attempt
        public static DataTable GetQuizMcqs(string quizId)
        {
            return AttemptQuizDL.GetQuizMcqs(quizId);
        }

        public static DataTable GetQuizShqs(string quizId)
        {
            return AttemptQuizDL.GetQuizShqs(quizId);
        }

        // Create a new attempt record
        public static bool CreateAttempt(attempt_model attempt)
        {
            try
            {
                attemptModel attemptDL = new attemptModel
                {
                    quizID = Convert.ToInt32(attempt.quizID),
                    subjectID = Convert.ToInt32(attempt.subjectID),
                    studentID = Convert.ToInt32(attempt.studentID)
                };

                // Check if student has already attempted this quiz
                bool hasAttempted = AttemptQuizDL.HasAttemptedQuiz(attemptDL.studentID, attemptDL.quizID);

                if (hasAttempted)
                {
                    Console.WriteLine("Student has already attempted this quiz");
                    return false;
                }

                return AttemptQuizDL.CreateAttempt(attemptDL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating attempt: " + ex.Message);
                return false;
            }
        }

        // Save MCQ answer
        public static bool SaveMcqAnswer(mcq_answer_model answer)
        {
            try
            {
                mcqAnswerModel answerDL = new mcqAnswerModel
                {
                    mcqID = Convert.ToInt32(answer.mcqID),
                    studentID = Convert.ToInt32(answer.studentID),
                    answer = answer.answer // A, B, C, or D
                };

                return AttemptQuizDL.SaveMcqAnswer(answerDL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving MCQ answer: " + ex.Message);
                return false;
            }
        }

        // Save Short Question answer
        public static bool SaveShqAnswer(shq_answer_model answer)
        {
            try
            {
                shqAnswerModel answerDL = new shqAnswerModel
                {
                    shqID = Convert.ToInt32(answer.shqID),
                    studentID = Convert.ToInt32(answer.studentID),
                    answer = answer.answer
                };

                return AttemptQuizDL.SaveShqAnswer(answerDL);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving SHQ answer: " + ex.Message);
                return false;
            }
        }

        // Submit complete quiz answers and calculate score
        public static bool SubmitQuiz(result_model result)
        {
            try
            {
                int quizId = Convert.ToInt32(result.quizID);
                int studentId = Convert.ToInt32(result.studentID);

                // Calculate MCQ score
                int mcqScore = AttemptQuizDL.CalculateMcqScore(studentId, quizId);

                // SHQ score is initially 0 since it requires manual grading
                int shqScore = 0;
                int totalScore = mcqScore + shqScore;

                // Create result record
                resultModel resultDL = new resultModel
                {
                    quizID = quizId,
                    studentID = studentId,
                    mcq_marks = mcqScore,
                    shq_marks = shqScore,
                    total_marks = totalScore
                };

                // Save results
                bool resultSaved = AttemptQuizDL.SaveQuizResults(resultDL);

                if (resultSaved)
                {
                    // Mark the quiz as attempted in the quiz table
                    AttemptQuizDL.MarkQuizAsAttempted(quizId);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error submitting quiz: " + ex.Message);
                return false;
            }
        }

        // Check if student has already attempted a quiz
        public static bool HasStudentAttemptedQuiz(string quizId, string studentId)
        {
            try
            {
                int quizIdInt = Convert.ToInt32(quizId);
                int studentIdInt = Convert.ToInt32(studentId);

                return AttemptQuizDL.HasAttemptedQuiz(studentIdInt, quizIdInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking quiz attempt: " + ex.Message);
                return false;
            }
        }
    }
}