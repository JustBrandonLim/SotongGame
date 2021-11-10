using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyStudentServer.Models
{
    public class Answer
    {
        public List<int> Answers { get; set; }

        public Answer() 
        {
            Answers = new List<int>();
        }
    }
}
