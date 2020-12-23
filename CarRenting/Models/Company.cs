﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    
    public class Company
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "É necessário indicar o nome da empresa")]
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

        public ICollection<Employee> Employees { get; set; }
      
        public ICollection<Car> Cars { get; set; }

    }

}