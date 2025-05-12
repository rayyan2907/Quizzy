﻿using Microsoft.AspNetCore.Mvc;
using Quizzy.Models.Buisness_Layer.quiz;
using Quizzy.Models.Buisness_Models;
using System;
using System.Data;
using System.Collections.Generic;
using Quizzy.Models.Buisness_Layer.subjects;
using Quizzy.Models.Buisness_Layer.student;
using Quizzy.Helpers;

namespace Quizzy.Controllers
{
    public class QuizAttemptController : Controller
    {
        public IActionResult AttemptQuiz(string quizId)
        {
            Console.WriteLine(quizId);

            // Get student information from session
            var stu = HttpContext.Session.GetObject<Student>("StudentObj");
            var subject = HttpContext.Session.GetObject<subject_model>("subObj");
            Console.WriteLine($"sttduent with name {stu.last_name} hs opened quiz {quizId} ");
            Console.WriteLine($"with sub {subject.name}");


            if (stu == null || subject == null)
            {
                TempData["log"] = "Session not found";

            }
            // Get quiz details
            quiz_model quiz = createQuizBL.getQuizObj(quizId);

            HttpContext.Session.SetObject("QuizObj", quiz);


            if (quiz == null)
            {
                TempData["log"] = "Session not found";

            }

            DataTable mcqs = AttemptQuizBL.GetQuizMcqs(quizId);
            DataTable shqs = AttemptQuizBL.GetQuizShqs(quizId);

            // Set ViewBag data
            ViewBag.stu = stu;
            ViewBag.sub = subject;
            ViewBag.QuizData = quiz;
            ViewBag.mcq = mcqs;
            ViewBag.shq = shqs;

            return View("QuizDetails"); 
        }



      

        [HttpPost]
        public IActionResult CreateAttempt([FromBody] attempt_model model)
        {
            try
            {
                string result = AttemptQuizBL.CreateAttempt(model);
                
                if (result.StartsWith("Error:") || result == "You have already attempted this quiz" || result == "Failed to create quiz attempt")
                {
                    return Json(new { success = false, message = result });
                }
                
                return Json(new { success = true, attemptId = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // API endpoint to save MCQ answer
        [HttpPost]
        public IActionResult SaveMcqAnswer([FromBody] mcq_answer_model model)
        {
            try
            {
                bool success = AttemptQuizBL.SaveMcqAnswer(model);
                
                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save answer" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // API endpoint to save Short Question answer
        [HttpPost]
        public IActionResult SaveShqAnswer([FromBody] shq_answer_model model)
        {
            try
            {
                bool success = AttemptQuizBL.SaveShqAnswer(model);
                
                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save answer" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // API endpoint to submit complete quiz
        [HttpPost]
        public IActionResult SubmitQuiz([FromBody] result_model model)
        {
            try
            {
                string result = AttemptQuizBL.SubmitQuiz(model);
                
                if (result == "Quiz submitted successfully")
                {
                    return Json(new { success = true, message = result });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // Display quiz results after submission
        public IActionResult QuizResults(string quizId)
        {
            try
            {
                var student = HttpContext.Session.GetObject<Student>("studentObj");

                if (string.IsNullOrEmpty(student.stuID))
                {
                    return RedirectToAction("index", "login");
                }

                // Get result details
                // You'd need to implement a method to get result details
                // This would typically be a call to a method in your business layer

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Failed to load quiz results: " + ex.Message;
                return View("Error");
            }
        }
    }
}