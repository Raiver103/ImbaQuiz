using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
