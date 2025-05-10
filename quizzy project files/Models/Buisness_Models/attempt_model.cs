namespace Quizzy.Models.Buisness_Models
{
    public class attempt_model
    {
        public string attemptID { get; set; }
        public string quizID { get; set; }
        public string subjectID { get; set; }
        public string studentID { get; set; }
    }

    public class mcq_answer_model
    {
        public string mcqID { get; set; }
        public string studentID { get; set; }
        public string answer { get; set; }  // A, B, C, or D
    }

    public class shq_answer_model
    {
        public string shqID { get; set; }
        public string studentID { get; set; }
        public string answer { get; set; }  // Text answer
    }

    public class result_model
    {
        public string resultID { get; set; }
        public string mcq_marks { get; set; }
        public string shq_marks { get; set; }
        public string total_marks { get; set; }
        public string quizID { get; set; }
        public string studentID { get; set; }
    }

    public class quiz_attempt_view_model
    {
        public quiz_model Quiz { get; set; }
        public Student Student { get; set; }
        public subject_model Subject { get; set; }
        public System.Data.DataTable Mcqs { get; set; }
        public System.Data.DataTable Shqs { get; set; }
    }
}

