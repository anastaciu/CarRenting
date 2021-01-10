using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using CarRenting.Attributes;

namespace CarRenting.Models
{
    
    public class Rent
    {
        
        public int Id { get; set; }
        [Required]
        [MinDate(ErrorMessage = "Datas inferiores à atual são inválidas")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data de início")]
        public DateTime Begin { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data de fim")]
        public DateTime End { get; set; }
        [ForeignKey("ApplicationUser")] 
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
        [Display(Name = "Reserva confirmada")]
        public bool IsConfirmed { get; set; }
        [Display(Name = "Entregue ao cliente")]
        public bool IsDelivered { get; set; }
        [Display(Name = "Devolvido pelo cliente")]
        public bool IsReceived { get; set; }
        [Display(Name = "Verificação de entrega efetuada")]
        public bool IsChecked { get; set; }
        [Display(Name = "Defeitos na entrega")]
        public string DeliveryFaults { get; set; }
        [Display(Name = "Kms ao receber")]
        public int KmsIn { get; set; }
        [Display(Name = "Kms na entrega")]
        public int KmsOut { get; set; }
        [Display(Name = "Danos ao receber")]
        public bool IsDamaged { get; set; }
        public ICollection<DamageImage> DamageImages { get; set; }

    }
}