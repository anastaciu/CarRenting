﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRenting.Attributes;

namespace CarRenting.Models
{
    
    public class Rent
    {
        
        public int Id { get; set; }
        [Required]
        [MinDate(ErrorMessage = "Só pode fazer reservas a partir de amanhã")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data de início")]
        public DateTime Begin { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Display(Name = "Data de fim")]
        public DateTime End { get; set; }
        [Required]
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
        [Display(Name = "Verificações efetuadas")]
        public bool IsChecked { get; set; }
        [Display(Name = "Defeitos na entrega")]
        public string DeliveryFaults { get; set; }
        [Display(Name = "Kms ao receber")]
        public int KmsIn { get; set; }
        [Display(Name = "Kms na entrega")]
        public int KmsOut { get; set; }
        [Display(Name = "Danos ao receber")]
        public bool IsDamaged { get; set; }
        [Display(Name = "Imagens dos danos")]
        public ICollection<DamageImage> DamageImages { get; set; }

    }
}