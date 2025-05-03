using Quizzy.Models.Data_Layer.quiz;
using Quizzy.Models.Buisness_Layer.quiz;
using System.Security.Cryptography.X509Certificates;
using Quizzy.Models.Buisness_Models;
using Quizzy.Models.DataLayer_Models;
using System.Data;


namespace Quizzy.Models.Buisness_Layer.quiz
{
    public class createQuizBL
    {
        public static bool deleteQuiz(string id)
        {
            return createQuizDL.deleteQuiz(id);
        }

        public static bool assignQuiz(string id)
        {
            return createQuizDL.assignQuiz(id);
        }

        public static bool unassignQuiz(string id)
        {
            return createQuizDL.unassignQuiz(id);
        }



        public static string addQuiz(Models.Buisness_Models.quiz_model q)
        {
            int giventime;
            bool is_time = int.TryParse(q.given_time, out giventime);

            if (!is_time)
            {
                return "Time should be in numbers";
            }
            DataLayer_Models.quizModel quiz = new DataLayer_Models.quizModel();
            int subId = Convert.ToInt32(q.subID);

            quiz.subID = subId;
            quiz.quizName = q.quizName;
            quiz.isPublic = q.isPublic;
            quiz.isAssign = q.isAssign;
            quiz.given_time = giventime;
            quiz.attempt = q.attempt;


            if (createQuizDL.addQuiz(quiz))
            {
                return "The quiz has been created successfully";
            }
            else
            {
                return "Error in adding quiz";
            }
        }


        public static string updateQuiz(Models.Buisness_Models.quiz_model q)
        {
            int giventime;
            bool is_time = int.TryParse(q.given_time, out giventime);

            if (!is_time)
            {
                return "Time should be in numbers";
            }
            DataLayer_Models.quizModel quiz = new DataLayer_Models.quizModel();
            int subId = Convert.ToInt32(q.subID);

            if (q.quizID != null)
            {   
                quiz.quizID = Convert.ToInt32(q.quizID);
                Console.WriteLine(quiz.quizID); 
            }
            quiz.subID = subId;
            quiz.quizName = q.quizName;
            quiz.isPublic = q.isPublic;
            quiz.isAssign = q.isAssign;
            quiz.given_time = giventime;
            quiz.attempt = q.attempt;


            if (createQuizDL.updateQuiz(quiz))
            {
                return "The quiz has been updated successfully";
            }
            else
            {
                return "Error in updating quiz";
            }

            
        }
        public static bool addMcq(mcq_model m)
        {
            int quizID= Convert.ToInt32(m.quizID);

            mcqModel mcq = new mcqModel();
            mcq.quizID = quizID;
            Console.WriteLine(mcq.quizID);
            mcq.description = m.description;
            mcq.opt1= m.opt1;
            mcq.opt2= m.opt2;
            mcq.opt3= m.opt3;
            mcq.opt4= m.opt4;
            mcq.corr_opt= m.corr_opt;

            return createQuizDL.addMcq(mcq);
            
        }

        public static bool addShq(shq_model shq)
        {
            int quizID = Convert.ToInt32(shq.quizID);

            shortQuestionModel s=new shortQuestionModel();
            s.quizID = quizID;
            s.shortQuestion=shq.shortQuestion;
            return createQuizDL.addShq(s);

        }

        public static quiz_model getQuizObj(string id)
        {
            return createQuizDL.getQuizObj(id);
        }


        public static DataTable getMcqs(quiz_model q)
        {
            return createQuizDL.getMcqs(q);
        }

        public static DataTable getShqs(quiz_model q)
        {
           return createQuizDL.getShqs(q);
        }
        public static bool mcqDel(string id)
        {
            
            return createQuizDL.deleteMcq(id);
        }

        public static bool shqDel(string id)
        {
            return createQuizDL.deleteShq(id);
        }


        public static mcq_model getMCqobj(string id)
        {
            return createQuizDL.getmcqObj(id);
        }

        public static bool updateMcq(mcq_model mq)
        {
            return createQuizDL.updateMcq(mq);
        }


        public static bool updateSHQ(shq_model s)
        {
            return createQuizDL.updateSHQ(s);
        }

        public static shq_model getshqObj(string id)
        {
            return createQuizDL.getshqObj(id);
        }
    }
}