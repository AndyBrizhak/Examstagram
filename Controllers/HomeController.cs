using Examstagram.Data;
using Examstagram.Models;
using Examstagram.Models.ViewModels;
using Examstagram.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Questions = _db.Questions.Include(u => u.Category).Include(u => u.Answers),
                Categories = _db.Category,
                Answers = _db.Answers
            };
            return View(homeVM);
        }

        public IActionResult Exercise(int id)
        {
            List<ExerciseCart> exerciseCartsList = new List<ExerciseCart>();
            if (HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                exerciseCartsList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);
            }
            ExerciseVM ExerciseVM = new ExerciseVM()
            {
                Question = _db.Questions.Include(u => u.Category).Include(u => u.Answers)
                .Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };
            foreach(var item in exerciseCartsList)
            {
                if(item.ExerciseQuestionId == id)
                {
                    ExerciseVM.ExistsInCart = true;
                }
            }
            return View(ExerciseVM);
        }

        [HttpPost,ActionName("Exercise")]
        public IActionResult ExercisePost(int id)
        {
            List<ExerciseCart> exerciseCartsList = new List<ExerciseCart>();
            if(HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart)!=null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                exerciseCartsList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);
            }
            exerciseCartsList.Add(new ExerciseCart { ExerciseQuestionId = id });
            HttpContext.Session.Set(WC.SessionCart, exerciseCartsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ExerciseCart> exerciseCartsList = new List<ExerciseCart>();
            if (HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ExerciseCart>>(WC.SessionCart).Count() > 0)
            {
                exerciseCartsList = HttpContext.Session.Get<List<ExerciseCart>>(WC.SessionCart);
            }

            var itemToRemove = exerciseCartsList.SingleOrDefault(r => r.ExerciseQuestionId == id);
            if(itemToRemove != null)
            {
                exerciseCartsList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, exerciseCartsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
