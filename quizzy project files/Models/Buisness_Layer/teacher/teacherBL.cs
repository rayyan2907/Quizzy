﻿using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.student;
using Quizzy.Models.Data_Layer.subjects;
using Quizzy.Models.Data_Layer.teacher;
using System.Data;

namespace Quizzy.Models.Buisness_Layer.teacher
{
    public class teacherBL
    {
        public static Teacher getData(string id )
        {
            teacherDL teacherDL = new teacherDL();
            Teacher model = new Teacher();
            model = teacherDL.getTeac( id );
            return model;
        }

        public DataTable getSub(Teacher t)
        {
            getSubject getSubject = new getSubject();
            DataTable table = getSubject.getSub(t.teachID);
            return table;   
        }

        public static DataTable annnounce(string id)
        {
            return teacherDL.announcement(id);
        }
        public static DataTable statsTotalstu(string id)
        {
            return teacherDL.statsTotalStu(id);
        }

        public static DataTable statsComplete(string id)
        {
            return teacherDL.statsCompQuiz(id);
        }
        public static DataTable statsUpcomming(string id)
        {
            return teacherDL.statsUpcomingQuiz(id);
        }
        public static DataTable statsAvg(string id)
        {
            return teacherDL.statsAggregate(id);
        }
    }
}
