using Examstagram.Data;
using Examstagram.Models;
using Examstagram.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QuestionController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Question> objList = _db.Questions.Include(u => u.Category).Include(u => u.Answers);
           /* foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryIdForQuestion);
                obj.Answer = _db.Answers.FirstOrDefault(u => u.Id == obj.AnswerIdForQuestion);
            };*/


            return View(objList);
        }


        //GET - CREATE
        public IActionResult Upsert(int? id)
        {
            /* IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
             {
                 Text = i.Name,
                 Value = i.Id.ToString()
             });
             ViewBag.CategoryDropDown = CategoryDropDown;*/

            QuestionVM questionVM = new QuestionVM()
            {
                Question = new Question(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                AnswerSelectList = _db.Answers.Select(i => new SelectListItem
                {
                    Text = i.AnswerContent,
                    Value = i.Id.ToString()
                })
            };
            if(id == null)
            {
                return View(questionVM);
            }

            else
            {
                questionVM.Question = _db.Questions.Find(id);
                if(questionVM.Question == null)
                {
                    return NotFound();
                }
                return View(questionVM);
            }
        }



        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(QuestionVM questionVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (questionVM.Question.Id == 0)
                {
                    string upload = webRootPath + WC.imagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStram = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStram);
                    }
                    questionVM.Question.Image = fileName + extension;
                    _db.Questions.Add(questionVM.Question);

                }
                else
                {
                    var objFromDb = _db.Questions.AsNoTracking().FirstOrDefault(u => u.Id == questionVM.Question.Id);
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.imagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        using (var fileStram = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStram);
                        }
                        questionVM.Question.Image = fileName + extension;
                    }
                    else
                    {
                        questionVM.Question.Image = objFromDb.Image;
                    }
                    _db.Questions.Update(questionVM.Question);  
                }
        
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            questionVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            questionVM.AnswerSelectList = _db.Answers.Select(i => new SelectListItem
            {
                Text = i.AnswerContent,
                Value = i.Id.ToString()
            });
            return View(questionVM);
        }

       
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Question question = _db.Questions.Include(u => u.Category).Include(u => u.Answers).FirstOrDefault(u => u.Id == id); //Include(u => u.Answer)
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Questions.Find(id);
            if (obj == null)
            {
                return NotFound();

            }
            string upload = _webHostEnvironment.WebRootPath + WC.imagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Questions.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
