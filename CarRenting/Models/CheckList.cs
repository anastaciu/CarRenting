using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class CheckList
    {
        [ForeignKey("Rent")]
        public int Id { get; set; }
        [Display(Name = "Danos nos pneus")]
        public bool Tires { get; set; }
        [Display(Name = "Danos nna carroçaria")]
        public bool Body { get; set; }
        [Display(Name = "Problemas mecânicos")]
        public bool Mechanics { get; set; }
        [Display(Name = "Danos no interior")]
        public bool Interior { get; set; }
        [Display(Name = "Danos no chassis")]
        public bool UnderBody { get; set; }
        [Display(Name = "Danos nos Vidros")]
        public bool Windows { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFile DamageImage { get; set; } 
        public virtual Rent Rent { get; set; }
    }
}