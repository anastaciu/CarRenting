using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class DamageImage
    {
        public int Id { get; set; }
        [ForeignKey("Rent")]
        public int RentId { get; set; }
        public string ImagePath { get; set; }
        public Rent Rent { get; set; }
    }
}