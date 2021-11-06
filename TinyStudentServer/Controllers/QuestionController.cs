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

        // GET api/question/{questionIndex} 
        [Route("api/question/{questionIndex}")]
        [HttpGet]
        public Question GetQuestion(int questionIndex)
        {
            return questions[questionIndex];
        }

        // POST api/question/
        [Route("api/question")]
        [HttpPost]
        public IHttpActionResult AddQuestion([FromBody] Question question)
        {
            questions.Add(question);

            return Ok();
        }
    }
}
