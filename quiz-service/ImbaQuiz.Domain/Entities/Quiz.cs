namespace ImbaQuiz.Domain.Entities
{
    public class Quiz : BaseEntity
    { 
        public string Title { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
