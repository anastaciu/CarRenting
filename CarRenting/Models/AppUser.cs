using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRenting.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Rent> Rents { get; set; }
    }
}