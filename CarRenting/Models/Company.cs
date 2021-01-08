using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CarRenting.Attributes;

namespace CarRenting.Models
{
    
    public class Company
    {
        public int Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "É necessário indicar o nome da empresa")]
        [UniqueCompany(ErrorMessage = "Já existe uma empresa com o mesmo nome")]
        public string CompanyName { get; set; }
        [Display(Name = "Telefone")]
        [Phone]
        public string CompanyPhoneNumber { get; set; }        
        [Display(Name = "Telemóvel")]
        [Phone]
        public string CompanyCellNumber { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "É necessário indicar o e-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }

}