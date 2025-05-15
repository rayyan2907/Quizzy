using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Models;
using System;
using System.Data;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Helpers;
using DBHelper;

namespace Quizzy.Controllers
{
    public class QuizAttemptController : Controller
    {
        private bool CheckIfResultExists(string quizID, string studentID)
        {
            try
            {
                int quizIdInt = Convert.ToInt32(quizID);
                int studentIdInt = Convert.ToInt32(studentID);

                string query = $"SELECT COUNT(*) FROM results WHERE quizID = {quizIdInt} AND studentID = {studentIdInt}";

                DataTable dt = DatabaseHelper.Instance.GetData(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(dt.Rows[0][0]);
                    return count > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if result exists: {ex.Message}");
                return false;
            }
        }
        public IActionResult AttemptQuiz(string quizId)
        {
            Console.WriteLine("Attempting quiz with ID: " + quizId);

            var student = HttpContext.Session.GetObject<Student>("StudentObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subObj");

            if (student == null || subject == null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }

            Console.WriteLine($"Student with name {student.first_name} {student.last_name} has opened quiz {quizId}");
            Console.WriteLine($"Subject: {subject.name}");

            quiz_model quiz = getQuizBL.getQuizdata(quizId);

            if (quiz == null)
            {
                TempData["log"] = "Quiz not found";
                return RedirectToAction("main", "student");
            }

            if (string.IsNullOrEmpty(quiz.quizID))
            {
                quiz.quizID = quizId;
                Console.WriteLine($"Fixed missing quiz ID, now set to: {quiz.quizID}");
            }

            bool hasSubmitted = CheckIfResultExists(quizId, student.stuID);

            if (hasSubmitted)
            {
                TempData["log"] = "You have already submitted this quiz.";
                return RedirectToAction("main", "student");
            }

            Console.WriteLine($"Quiz Model: ID={quiz.quizID}, Name={quiz.quizName}, Time={quiz.given_time}");

            HttpContext.Session.SetObject("QuizObj", quiz);

            DataTable mcqs = createQuizBL.getMcqs(quiz);
            DataTable shqs = createQuizBL.getShqs(quiz);

            ViewBag.stu = student;
            ViewBag.sub = subject;
            ViewBag.QuizData = quiz;
            ViewBag.mcq = mcqs;
            ViewBag.shq = shqs;
            ViewBag.HasSubmitted = hasSubmitted;  

            if (!hasSubmitted)
            {
                attempt_model attempt = new attempt_model
                {
                    quizID = quizId,
                    subjectID = subject.subjectID,
                    studentID = student.stuID
                };

                Console.WriteLine("Creating attempt record");
                AttemptQuizBL.CreateAttempt(attempt);
            }

            return View("attemptQuiz");
        }

        [HttpPost]
        public IActionResult CreateAttempt(attempt_model model)
        {
            Console.WriteLine($"Creating attempt for quiz ID: {model.quizID}, student ID: {model.studentID}");

            try
            {

                bool hasAttempted = AttemptQuizBL.HasStudentAttemptedQuiz(model.quizID, model.studentID);

                if (hasAttempted)
                {
                    TempData["log"] = "You have already attempted this quiz";
                    return RedirectToAction("Dashboard", "Student");
                }

              
                bool success = AttemptQuizBL.CreateAttempt(model);

                if (success)
                {
                  
                    return RedirectToAction("AttemptQuiz", new { quizId = model.quizID });
                }
                else
                {
                    TempData["log"] = "Failed to create quiz attempt";
                    return RedirectToAction("Dashboard", "Student");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating attempt: " + ex.Message);
                TempData["log"] = "Error: " + ex.Message;
                return RedirectToAction("Dashboard", "Student");
            }
        }

        [HttpPost]
        public IActionResult SaveMcqAnswer(mcq_answer_model model)
        {
            Console.WriteLine($"Saving MCQ answer: {model.mcqID}, answer: {model.answer}");

            try
            {
                bool result = AttemptQuizBL.SaveMcqAnswer(model);
                if (result)
                {
                    Console.WriteLine("MCQ answer saved successfully");
                    return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
                }
                else
                {
                    TempData["log"] = "Failed to save answer";
                    return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving MCQ answer: " + ex.Message);
                TempData["log"] = "Error saving answer: " + ex.Message;
                return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
            }
        }

        [HttpPost]
        public IActionResult SaveShqAnswer(shq_answer_model model)
        {
            Console.WriteLine($"Saving SHQ answer: {model.shqID}");

            try
            {
                bool result = AttemptQuizBL.SaveShqAnswer(model);
                if (result)
                {
                    Console.WriteLine("SHQ answer saved successfully");
                    return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
                }
                else
                {
                    TempData["log"] = "Failed to save answer";
                    return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving SHQ answer: " + ex.Message);
                TempData["log"] = "Error saving answer: " + ex.Message;
                return RedirectToAction("AttemptQuiz", new { quizId = HttpContext.Session.GetObject<quiz_model>("QuizObj").quizID });
            }
        }

        [HttpPost]
        public IActionResult SubmitQuiz(string quizID, string studentID)
        {
            Console.WriteLine($"Submitting quiz: {quizID} for student: {studentID}");

            try
            {
                var quiz = HttpContext.Session.GetObject<quiz_model>("QuizObj");
                var student = HttpContext.Session.GetObject<Student>("StudentObj");

                if (quiz == null || student == null)
                {
                    TempData["log"] = "Session not found";
                    return RedirectToAction("index", "login");
                }

            
                result_model result = new result_model
                {
                    quizID = quizID,
                    studentID = studentID
                };

               
                bool success = AttemptQuizBL.SubmitQuiz(result);

                if (success)
                {
                    TempData["Check"] = "Quiz submitted successfully";
                    return RedirectToAction("Dashboard", "Student");
                }
                else
                {
                    TempData["log"] = "Failed to submit quiz";
                    return RedirectToAction("AttemptQuiz", new { quizId = quizID });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error submitting quiz: " + ex.Message);
                TempData["log"] = "Error submitting quiz: " + ex.Message;
                return RedirectToAction("AttemptQuiz", new { quizId = quizID });
            }
        }
    }
}