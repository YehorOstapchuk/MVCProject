using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Group
    {
        public Group()
        {
            Aspirant = new HashSet<Aspirant>();
            Sessions = new HashSet<Sessions>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва групи")]
        [MaxLength(50)]

        [RegularExpression(@"[А-Я|Є|І|']{1}[0-9|-]+$", ErrorMessage = "Некорректне ім'я")]
        [Remote(action: "CheckGroup", controller: "Groups", ErrorMessage = "Назва вже використовується")]
        public string GroupName { get; set; }

        [Display(Name = "Тип навчання")]
        public int? TypeOfStudying { get; set; }
        [Display(Name = "Назва кафедри")]
        public int? CathedraId { get; set; }
        [Display(Name = "Назва кафедри")]
        public virtual Cathedras Cathedra { get; set; }
        public virtual ICollection<Aspirant> Aspirant { get; set; }
        public virtual ICollection<Sessions> Sessions { get; set; }
    }
}
