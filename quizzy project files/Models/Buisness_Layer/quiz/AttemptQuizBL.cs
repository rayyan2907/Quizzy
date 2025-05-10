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
        public static string CreateAttempt(attempt_model attempt)
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
                    return "You have already attempted this quiz";
                }

                if (AttemptQuizDL.CreateAttempt(attemptDL))
                {
                    // Get the attempt ID for the newly created attempt
                    int attemptId = AttemptQuizDL.GetLatestAttemptId(attemptDL.studentID, attemptDL.quizID);
                    return attemptId.ToString();
                }
                else
                {
                    return "Failed to create quiz attempt";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating attempt: " + ex.Message);
                return "Error: " + ex.Message;
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
        public static string SubmitQuiz(result_model result)
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
                if (AttemptQuizDL.SaveQuizResults(resultDL))
                {
                    // Mark the quiz as attempted in the quiz table
                    AttemptQuizDL.MarkQuizAsAttempted(quizId);
                    return "Quiz submitted successfully";
                }
                else
                {
                    return "Failed to submit quiz";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error submitting quiz: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        // Load quiz for attempting
        public static quiz_attempt_view_model LoadQuizForAttempt(string quizId, string studentId, string subjectId)
        {
            quiz_attempt_view_model viewModel = new quiz_attempt_view_model();

            // Get quiz details
            viewModel.Quiz = createQuizBL.getQuizObj(quizId);

            // Get subject details
            subject_model subject = new subject_model();
            // You'll need to implement a method to get subject details
            // This would typically be in a subject-related business layer class

            // Get student details
            Student student = new Student();
            // You'll need to implement a method to get student details
            // This would typically be in a student-related business layer class

            // Get MCQs and SHQs
            viewModel.Mcqs = GetQuizMcqs(quizId);
            viewModel.Shqs = GetQuizShqs(quizId);

            return viewModel;
        }

        // Get quiz review data
        public static Dictionary<string, DataTable> GetQuizReview(string quizId, string studentId)
        {
            Dictionary<string, DataTable> reviewData = new Dictionary<string, DataTable>();

            int quizIdInt = Convert.ToInt32(quizId);
            int studentIdInt = Convert.ToInt32(studentId);

            // Get MCQ answers with correct answers
            DataTable mcqReview = AttemptQuizDL.GetMcqAnswers(studentIdInt, quizIdInt);

            // Get SHQ answers
            DataTable shqReview = AttemptQuizDL.GetShqAnswers(studentIdInt, quizIdInt);

            reviewData.Add("mcqs", mcqReview);
            reviewData.Add("shqs", shqReview);

            return reviewData;
        }
    }
}