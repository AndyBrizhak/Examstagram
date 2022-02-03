using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models
{
    public class Answer
    {
  
        [Key]
        public int Id { get; set; }

        [DisplayName("Content")]
        [Required]
        public string AnswerContent { get; set; }

        [DisplayName("Correct (True are False")]
        [Required]
        public bool? AnswerCorrect { get; set; }




        [Display(Name = "Question Type")]
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Questions { get; set; }
    }
}
