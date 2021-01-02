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
        public string Description { get; set; }
        public virtual Rent Rent { get; set; }
    }
}