using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC
{
    public partial class Aspirant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Ім'я")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|']{1}[а-я|є|і|']+$", ErrorMessage = "Некорректне ім'я")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Прізвище")]
        [MaxLength(50)]
        [RegularExpression(@"[А-Я|Є|І|']{1}[а-я|є|і|']+$", ErrorMessage = "Некорректне ім'я")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        [Remote(action: "CheckBirth", controller: "Aspirants", ErrorMessage = "Некоректна дата")]
       // [Remote(action: "CheckBirth", controller: "Aspirants1", ErrorMessage = "Некоректна дата")]
        public DateTime? BirthDay { get; set; }
        [Display(Name = "Назва групи")]
        public int? GroupId { get; set; }

        [Display(Name = "Назва групи")]
        public virtual Group Group { get; set; }
    }
}
