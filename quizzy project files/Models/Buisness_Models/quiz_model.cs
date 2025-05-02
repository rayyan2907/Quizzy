namespace Quizzy.Models.Buisness_Models
{
    public class quiz_model
    {
        public string quizID { get; set; }
        public string quizName { get; set; }
        public bool isPublic { get; set; }
        public bool isAssign {  get; set; }
        public string  given_time {  get; set; }
        public bool attempt {  get; set; }
        public string subID { get; set; }
    }
}
