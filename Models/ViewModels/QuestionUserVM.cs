using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models.ViewModels
{
    public class QuestionUserVM
    {
        public QuestionUserVM()
        {
            QuestionList = new List<Question>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Question> QuestionList { get; set; }
    }
}
