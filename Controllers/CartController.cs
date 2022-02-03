using Examstagram.Data;
using Examstagram.Models;
using Examstagram.Models.ViewModels;
using Examstagram.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Examstagram.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public QuestionUserVM QuestionUserVM { get; set; }
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ExerciseCart> execiseCartList = new List<ExerciseCart>();
            if (HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                execiseCartList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);

            }

            List<int> questInCart = execiseCartList.Select(i => i.ExerciseQuestionId).ToList();
            IEnumerable<Question> questList = _db.Questions.Where(u => questInCart.Contains(u.Id));

            return View(questList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {


            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ExerciseCart> execiseCartList = new List<ExerciseCart>();
            if (HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                execiseCartList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);

            }

            List<int> questInCart = execiseCartList.Select(i => i.ExerciseQuestionId).ToList();
            IEnumerable<Question> questList = _db.Questions.Where(u => questInCart.Contains(u.Id));

            QuestionUserVM = new QuestionUserVM()
            {
              //  ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value)
            };
    
            return View(QuestionUserVM);
        }




        public IActionResult Remove(int id)
        {
            List<ExerciseCart> execiseCartList = new List<ExerciseCart>();
            if (HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                execiseCartList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);

            }
            execiseCartList.Remove(execiseCartList.FirstOrDefault(u => u.ExerciseQuestionId == id));
            HttpContext.Session.Set(WC.SessionCart, execiseCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
