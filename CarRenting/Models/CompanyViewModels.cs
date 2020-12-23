using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class CompanyViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Company Company { get; set; }
        public Employee Employee { get; set; }
    }
}