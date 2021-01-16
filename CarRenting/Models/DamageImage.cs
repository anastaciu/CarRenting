using System.ComponentModel.DataAnnotations.Schema;
using CarRenting.Attributes;

namespace CarRenting.Models
{
    public class DamageImage
    {
        public int Id { get; set; }
        [ForeignKey("Rent")]
        public int RentId { get; set; }
        public string ImagePath { get; set; }
        public Rent Rent { get; set; }
    }
}