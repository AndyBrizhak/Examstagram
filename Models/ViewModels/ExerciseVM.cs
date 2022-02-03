using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models.ViewModels
{
    public class ExerciseVM
    {
        public ExerciseVM()
        {
            Question = new Question();
            
        }
        public Question Question { get; set; }
        public bool ExistsInCart { get; set; }

    }
}
