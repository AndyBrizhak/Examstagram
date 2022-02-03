using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examstagram.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Название экзамена")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Описание экзамена")]
        [Required]
        public string Description { get; set; }


        [DisplayName("Question")]
        [Required]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
