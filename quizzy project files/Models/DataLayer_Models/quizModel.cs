namespace Quizzy.Models.DataLayer_Models
{
    public class quizModel
    {
        public int quizID { get; set; }
        public string quizName { get; set; }
        public bool isPublic { get; set; }
        public bool isAssign { get; set; }
        public int given_time { get; set; }
        public bool attempt { get; set; }
        public int subID { get; set; }
    }
}
