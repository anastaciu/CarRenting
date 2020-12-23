using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Rent
    {
        public int Id { get; set; }
        
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        
        public ApplicationUser User { get; set; }

        public Car Car { get; set; }


    }
}