using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRenting.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        public virtual ICollection<Rent> Rents { get; set; }
    }
}