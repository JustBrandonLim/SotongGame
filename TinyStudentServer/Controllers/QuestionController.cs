using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TinyStudentServer.Models;

namespace TinyStudentServer
{
    // api/question/
    public class QuestionController : ApiController
    {
        private static List<Question> questions = new List<Question>();
        
        private static List<(int, int)> answers = new List<(int, int)> ();

        [Route("api/question/getquestion/{questionIndex}")]
        [HttpGet]
        public Question GetQuestion(int questionIndex)
        {
            System.Diagnostics.Debug.WriteLine("GET GetQuestion({0})", questionIndex);

            if (questionIndex > questions.Count - 1)
                return new Question("NO MORE QUESTIONS", null, 0);

            return questions[questionIndex];
        }

        [Route("api/question/addquestion")]
        [HttpPost]
        public IHttpActionResult AddQuestion([FromBody] Question question)
        {
            System.Diagnostics.Debug.WriteLine("POST AddQuestion");
            System.Diagnostics.Debug.WriteLine("{0}, {1}, {2}", question.Content, question.Options, question.Answer);
            questions.Add(question);

            return Ok();
        }
    }
}
