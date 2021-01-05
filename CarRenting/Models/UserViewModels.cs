using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRenting.Models
{
    public class DashViewModel
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }

    }
    public class EmployeeViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Permissões")]
        public string RoleName { get; set; }

    }

    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }
        [Display(Name = "Nome")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Telefone/Telemóvel")]
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name = "Permissões")]
        [Required]
        public string Role { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

    }


}