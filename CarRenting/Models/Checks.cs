using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Checks
    {
        public int Id { get; set; }
        public string Description { get; set; }
        private ICollection<Car> Cars { get; set; }
    }
}