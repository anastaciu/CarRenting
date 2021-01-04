using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Rent
    {
        public int Id { get; set; }
        
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        
        [ForeignKey("ApplicationUser")] 
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }


    }
}