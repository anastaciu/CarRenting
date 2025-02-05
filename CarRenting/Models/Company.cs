﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRenting.Models
{
    
    public class Company
    {
        public int Id { get; set; }
        [Display(Name = "Empresa")]
        [Required]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "Contribuinte")]
        public string NiF { get; set; }
        [Display(Name = "Telefone")]
        [Phone]
        public string CompanyPhoneNumber { get; set; }        
        [Display(Name = "Telemóvel")]
        [Phone]
        public string CompanyCellNumber { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public virtual ICollection<Check> Checks { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

    }

}