using DBHelper;
using Quizzy.Models.DataLayer_Models;

namespace Quizzy.Models.Data_Layer.subjects
{
    public class addSub
    {
        public static bool addSubject(subjectModel subject)
        {
            int rows=0;
            try
            {
                string query = $"insert into subjects (subject_name,subject_code,teacherID) values ('{subject.name}','{subject.code}','{subject.teacherID}') ";
                rows = DatabaseHelper.Instance.Update(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return rows > 0;
        }
    }
}
