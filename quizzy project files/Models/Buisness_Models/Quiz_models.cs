namespace Quizzy.Models.Buisness_Models
{
    public class Quiz_models
    {
        public int mcq_id { get; set; }
        public string description { get; set; }
        public string opt1 { get; set; }
        public string opt2 { get; set; }
        public string? opt3 { get; set; }
        public string? opt4 { get; set; }
        public string corr_opt { get; set; }
        public string? img_path { get; set;}
    }
    public class short_model
    {
        public string question { get; set; }
        public string? image_path { get; set; }
    }

}
