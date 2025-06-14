﻿namespace ImbaQuiz.Domain.Entities
{
    public class User 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>(); 
    }
}
