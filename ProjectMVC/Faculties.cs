using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Faculties
    {
        public Faculties()
        {
            Cathedras = new HashSet<Cathedras>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва факультету")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|а-я|є|і|'| ]+$", ErrorMessage = "Некорректне ім'я")]
        [Remote(action: "CheckFaculty", controller: "Faculties", ErrorMessage = "Назва вже використовується")]
        public string FacultyName { get; set; }

        public virtual ICollection<Cathedras> Cathedras { get; set; }
    }
}
