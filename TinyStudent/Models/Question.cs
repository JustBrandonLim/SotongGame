using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyStudent.Models
{
    public class Question
    {
        public string Content { get; set; }
        public List<string> Options { get; set; }
        public int Answer { get; set; }

        public Question(string content, List<string> options, int answer)
        {
            Content = content;
            Options = options;
            Answer = answer;
        }
    }
}
