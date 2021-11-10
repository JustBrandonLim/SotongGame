using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyStudent.Models
{
    public class Answer
    {
        public List<int> Answers { get; set; }

        public Answer(List<int> answers)
        {
            Answers = answers;
        }
    }
}
