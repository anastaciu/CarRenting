using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Car
    {
        
        public int Id { get; set; }
        public string Model { get; set; }
        public string Fuel { get; set; }
        public int Seats { get; set; }

        [ForeignKey("Company")] 
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public ICollection<Rent> Rents { get; set; }

    }
}