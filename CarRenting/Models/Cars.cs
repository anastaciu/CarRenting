using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace CarRenting.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Display(Name = "Marca")]
        public string Marca { get; set; }
        [Display(Name = "Modelo")]
        public string Model { get; set; }
        [Display(Name = "Combustível")]
        public string Fuel { get; set; } 
        [Required] 
        public string FuelLevel { get; set; }
        [Display(Name = "Lugares")]
        [Range(1, 200)]
        public int Seats { get; set; }
        [ForeignKey("Type")] 
        public int TypeId { get; set; }
        public CarType Type { get; set; }
        [Display(Name = "Preço/Dia")]
        public double Price { get; set; }
        [Range(0, Int32.MaxValue)]
        public int Kms { get; set; }
        [ForeignKey("Company")] 
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Rent> Rents { get; set; }
        public ICollection<Checks> Checks { get; set; }

        public Car()
        {
            FuelLevel = "Cheio";
        }
    }
}