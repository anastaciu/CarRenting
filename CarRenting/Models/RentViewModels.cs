using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class ReceptionViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Verificações efetuadas")]
        public bool IsChecked { get; set; }
        [Display(Name = "Veículo com danos")]
        public bool IsDamaged { get; set; }
        [Required]
        [Display(Name = "Kms na recepção")]
        public int KmsIn { get; set; }
        public string FuelLevel { get; set; }
        [Display(Name = "Carregar imagens de danos")]
        public HttpPostedFileBase[] Files { get; set; }
        [Display(Name = "Marca")]
        public string CarBrand { get; set; }
        [Display(Name = "Modelo")]
        public string CarModel { get; set; }
        [Display(Name = "Matrícula")]
        public string CarLicense { get; set; }
        [Display(Name = "Lista de verificações")]
        public List<Check> Checks { get; set; }

    }



}