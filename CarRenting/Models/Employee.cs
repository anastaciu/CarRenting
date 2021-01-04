using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace CarRenting.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        [ForeignKey("Company")] 
        public int CompanyId { get; set; }
        
        public Company Company{ get; set; }

    }
}