using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class CarType
    {
        public int Id { get; set; }
        [Display(Name = "Tipo de Veículo")]
        public string Type { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}