using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Schedule
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Номер сессії")]
        public int? SessionId { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва предмету")]
        public int? ExamId { get; set; }

        [Display(Name = "Назва предмету")]
        public virtual Exams Exam { get; set; }

        [Display(Name = "Номер сессії")]
        public virtual Sessions Session { get; set; }
    }
}
