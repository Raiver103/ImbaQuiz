using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Entities
{
    internal class Question
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string? Picture { get; set; }
        public ICollection<string> Answers { get; set; } = new List<string>();
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}
