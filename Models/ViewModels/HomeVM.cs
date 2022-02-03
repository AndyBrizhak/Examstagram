using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Answer> Answers { get; set; }

    }
}
