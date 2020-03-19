using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Cathedras
    {
        public Cathedras()
        {
            Group = new HashSet<Group>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва кафедри")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|']+$", ErrorMessage = "Некорректне ім'я")]
        [Remote(action: "CheckCathedra", controller: "Cathedras", ErrorMessage = "Назва вже використовується")]
        public string CathedraName { get; set; }
        public int? FacultyId { get; set; }
        [Display(Name = "Назва факультету")]
        public virtual Faculties Faculty { get; set; }
        public virtual ICollection<Group> Group { get; set; }
    }
}
