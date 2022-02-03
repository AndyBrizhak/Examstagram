using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models.ViewModels
{
    public class ExamVM
    {
        public Question Question { get; set; }
        public Category Category { get; set; }
        public Answer Answer { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> AnswerSelectList { get; set; }
        public IEnumerable<SelectListItem> QuestionSelectList { get; set; }
    }
}
