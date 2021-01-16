using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CarRenting.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Display(Name = "Matrícula")]
        [Required]
        public string License { get; set; }
        [Display(Name = "Marca")]
        [Required]
        public string Brand { get; set; }
        [Display(Name = "Modelo")]
        [Required]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Combustível")]
        public string Fuel { get; set; } 
        [Required] 
        [Display(Name = "Nível do depósito")]
        public string FuelLevel { get; set; }
        [Display(Name = "Lugares")]
        [Range(1, 200)]
        [Required]
        public int Seats { get; set; }
        [ForeignKey("Type")]
        [Display(Name = "Categoria")]
        public int TypeId { get; set; }
        [Display(Name = "Categoria")]
        public CarType Type { get; set; }
        [Display(Name = "Preço/Dia")]
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int Kms { get; set; }
        [ForeignKey("Company")] 
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        [Display(Name = "Reservas")]
        public ICollection<Rent> Rents { get; set; }
        [Display(Name = "Fotos da viatura")]
        public ICollection<CarImage> CarImages { get; set; }
        [Display(Name = "Fotos da viatura")]
        public HttpPostedFileBase[] Files { get; set; }
        public Car()
        {
            FuelLevel = "Cheio";
        }
    }
}