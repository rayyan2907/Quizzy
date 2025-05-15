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

        public IActionResult showQuiz(string quizId)
        {
            Console.WriteLine("Attempting quiz with ID: " + quizId);

            var student = HttpContext.Session.GetObject<Student>("StudentObj");

            if (student == null)
            {
                TempData["log"] = "Session not found";
                return RedirectToAction("index", "login");
            }

            


            Console.WriteLine($"Student with name {student.first_name} {student.last_name} has opened quiz {quizId}");

            quiz_model quiz = getQuizBL.getQuizdata(quizId);

            if (quiz == null)
            {
                TempData["log"] = "Quiz not found";
                return RedirectToAction("main", "student");
            }

            subject_model subject = new subject_model();
           

            subject = subjectBL.getSubfromid(quiz.subID);

            
            Console.WriteLine($"Subject: {subject.name} id is {quiz.subID}");


            Console.WriteLine($"Quiz Model: ID={quiz.quizID}, Name={quiz.quizName}, Time={quiz.given_time}");

            HttpContext.Session.SetObject("QuizObj", quiz);


            ViewBag.stu = student;
            ViewBag.sub = subject;
            ViewBag.QuizData = quiz;

            return View("QuizDetails");
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


            Console.WriteLine($"Quiz Model: ID={quiz.quizID}, Name={quiz.quizName}, Time={quiz.given_time}");

            HttpContext.Session.SetObject("QuizObj", quiz);

            DataTable mcqs = createQuizBL.getMcqs(quiz);
            DataTable shqs = createQuizBL.getShqs(quiz);

            ViewBag.stu = student;
            ViewBag.sub = subject;
            ViewBag.QuizData = quiz;
            ViewBag.mcq = mcqs;
            ViewBag.shq = shqs;
           

            bool hasAttempted = AttemptQuizBL.HasStudentAttemptedQuiz(quizId, student.stuID);

            if (hasAttempted)
            {
                TempData["log"] = "You have already attempted this quiz";
                return RedirectToAction("main", "student");
            }
            Console.WriteLine("no attempt found");

            attempt_model attempt = new attempt_model
            {
                quizID = quizId,
                subjectID = subject.subjectID,
                studentID = student.stuID
            };
            bool flag = AttemptQuizBL.CreateAttempt(attempt);
            if (!flag)
            {
                TempData["log"] = "error in making attempt";
                return RedirectToAction("main", "student");
            }
            Console.WriteLine("Creating attempt record");

            return View("attemptQuiz");
        }

        [HttpPost]
        public IActionResult CreateAttempt(attempt_model model)
        {
            Console.WriteLine($"Creating attempt for quiz ID: {model.quizID}, student ID: {model.studentID}");

            try
            {

                

                if (true)
                {
                  
                    return RedirectToAction("AttemptQuiz", new { quizId = model.quizID });
                }
                else
                {
                    TempData["log"] = "Failed to create quiz attempt";
                    return RedirectToAction("main", "student");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating attempt: " + ex.Message);
                TempData["log"] = "Error: " + ex.Message;
                return RedirectToAction("main", "student");
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
                    return RedirectToAction("main", "student");
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