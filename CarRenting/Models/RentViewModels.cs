using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{

    public class DeliveryViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Defeitos na entrega")]
        public string DeliveryFaults { get; set; }
        [Required]
        [Display(Name = "Kms na entrega")]
        public int KmsOut { get; set; }
        public string FuelLevel { get; set; }
    }

}