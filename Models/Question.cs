using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models
{
    public class Question
    {

      
        [Key]
        public int Id { get; set; }

        [DisplayName("Вопрос")]
        [Required]
        public string QuestionContent { get; set; }

        public string Explaining { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }



    /*    [Display(Name = "id вопроса")]
        [Range(1, int.MaxValue, ErrorMessage = "Недопустимый id")]
        public int? AnswerIdForQuestion { get; set; }
        [ForeignKey("AnswerIdForQuestion")]
        public virtual Answer Answer { get; set; }*/




        [Display(Name = "CategoryIdForQuestion")]
        public int? CategoryIdForQuestion { get; set; }
        [ForeignKey("CategoryIdForQuestion")]
        public virtual Category Category { get; set; }




        [DisplayName("Answer")]
        [Required]
        public virtual ICollection<Answer> Answers { get; set; }





    }
}
