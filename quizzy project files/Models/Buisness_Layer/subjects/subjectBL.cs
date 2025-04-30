using Quizzy.Models.Buisness_Models;
using Quizzy.Models.Data_Layer.subjects;
using Quizzy.Models.DataLayer_Models;

namespace Quizzy.Models.Buisness_Layer.subjects
{
    public class subjectBL
    {
        public string subjectAdd(subject_model sub)
        {
            
            int teachID;
            bool isTeach = int.TryParse(sub.teacherID, out teachID);
            if (!isTeach)
            {
                return "Error in adding subject. Conversion not possible";
            }

            subjectModel model = new subjectModel();
            model.teacherID = teachID;
            model.name= sub.name;
            model.code = sub.code;

            bool flag = addSub.addSubject(model);

            if (flag)
            {
                Console.WriteLine($"a new subject {sub.name} and code {sub.code} has been added");

                return "Subject added successfully";
            }
            else
            {
                return "Error in adding subject";
            }
        }
    }
}
