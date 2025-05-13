namespace ImbaQuiz.Domain.Entities
{
    public class Question : BaseEntity
    { 
        public string Text { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
