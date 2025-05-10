namespace Quizzy.Models.DataLayer_Models
{
    public class attemptModel
    {
        public int attemptID { get; set; }
        public int quizID { get; set; }
        public int subjectID { get; set; }
        public int studentID { get; set; }
    }

    public class mcqAnswerModel
    {
        public int mcqID { get; set; }
        public int studentID { get; set; }
        public string answer { get; set; }  // A, B, C, or D
    }

    public class shqAnswerModel
    {
        public int shqID { get; set; }
        public int studentID { get; set; }
        public string answer { get; set; }  // Text answer
    }

    public class resultModel
    {
        public int resultID { get; set; }
        public int mcq_marks { get; set; }
        public int shq_marks { get; set; }
        public int total_marks { get; set; }
        public int quizID { get; set; }
        public int studentID { get; set; }
    }
}
