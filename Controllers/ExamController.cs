using Examstagram.Data;
using Examstagram.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Controllers
{
    public class ExamController : Controller
    {
      /*  private ApplicationDbContext db = new ApplicationDbContext();*/

      private ApplicationDbContext _db;


        public ExamController(ApplicationDbContext db)
      {
           _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);

        }

        public IActionResult StartExam()
          {
            IEnumerable<Question> objList = _db.Questions.Include(u => u.Answers).Include(u => u.Category);
            return View(objList);
          }

        [HttpPost]
        public IActionResult ExamResults(IFormCollection iformCollection)
        {

            int score = 0;
            string[] questionsId = iformCollection["QuestionId"];
            foreach (var questionId in questionsId)
            {

                int answerIdCorrect = _db.Questions.Find(int.Parse
                   (questionId)).Answers.Where(a => a.AnswerCorrect == true).FirstOrDefault().Id;

                if (answerIdCorrect == int.Parse(iformCollection["Question" + questionId]))
                {
                    score++;
                }
            }
            ViewBag.score = score;
            return View();
        }



    }
}
