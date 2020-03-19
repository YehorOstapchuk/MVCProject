using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Exams
    {
        public Exams()
        {
            Schedule = new HashSet<Schedule>();
        }

        public int Id { get; set; }

       [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва предмету")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|а-я|є|і|'| ]+$", ErrorMessage = "Некорректне ім'я")]
        [Remote(action: "CheckExam", controller: "Exams", ErrorMessage = "Назва вже використовується")]
        public string ExamName { get; set; }

      // [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Дата екзамену")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }

        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
