using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Checks
    {
        public Checks()
        {
            this.Rents = new HashSet<Rent>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Rent> Rents { get; set; }
    }
}