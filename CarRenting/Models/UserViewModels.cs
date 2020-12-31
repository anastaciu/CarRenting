using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class CompanyDashViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Company Company { get; set; }
        public string Role;

    }
    public class UserDashViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Permissões")]
        public string Role;

    }


}