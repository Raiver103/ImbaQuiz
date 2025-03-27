using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Domain.Entities
{
    public class Answer
    { 
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }  
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
