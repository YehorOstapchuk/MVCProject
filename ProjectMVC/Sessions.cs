using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Sessions
    {
        public Sessions()
        {
            Schedule = new HashSet<Schedule>();
        }
        [Display(Name = "Назва сессії")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|']{1}[а-я|є|і|']+$", ErrorMessage = "Некорректне ім'я")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва групи")]
        public int? GroupId { get; set; }
       // [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва сессії")]
        public string SessionName { get; set; }
        [Display(Name = "Група")]
        public virtual Group Group { get; set; }
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}
