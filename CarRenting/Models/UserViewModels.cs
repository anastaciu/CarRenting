using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public string Role { get; set; }

    }


}