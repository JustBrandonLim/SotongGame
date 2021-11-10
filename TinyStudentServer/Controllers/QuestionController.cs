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
        //Current Question
        private static int questionIndex = 0;

        private static List<Question> questions = new List<Question>();
        private static List<Answer> answers = new List<Answer>();

        #region GET
        [Route("api/question/getquestionindex")]
        [HttpGet]
        public int GetQuestionIndex()
        {
            System.Diagnostics.Debug.WriteLine("GET GetQuestionIndex()");

            return questionIndex;
        }

        [Route("api/question/getquestion")]
        [HttpGet]
        public Question GetQuestion()
        {
            System.Diagnostics.Debug.WriteLine("GET GetQuestion()");

            if (questions.Count - 1 < questionIndex)
                return new Question("NO AVAILABLE QUESTIONS", null, 0);

            var question = questions[questionIndex];
            questionIndex++;

            return question;
        }

        [Route("api/question/getsubmissions")]
        [HttpGet]
        public Answer GetSubmissions()
        {
            System.Diagnostics.Debug.WriteLine("GET GetSubmissions()");

            if (questions.Count == 0)
                return new Answer();

            return answers[questionIndex];
        }
        #endregion

        #region POST
        [Route("api/question/addquestion")]
        [HttpPost]
        public IHttpActionResult AddQuestion([FromBody] Question question)
        {
            System.Diagnostics.Debug.WriteLine("POST AddQuestion()");
            System.Diagnostics.Debug.WriteLine("{0}, {1}, {2}", question.Content, question.Options, question.Answer);
            
            questions.Add(question);
            answers.Add(new Answer());

            return Ok();
        }

        [Route("api/question/submitanswer/{answer}")]
        [HttpPost]
        public IHttpActionResult SubmitAnswer(int answer)
        {
            System.Diagnostics.Debug.WriteLine("POST SubmitAnswer({0})", answer);

            answers[questionIndex].Answers.Add(answer);

            return Ok();
        }
        #endregion
    }
}
