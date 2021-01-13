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
        [Required]
        [Display(Name = "Kms na recepção")]
        public int KmsIn { get; set; }
        public string FuelLevel { get; set; }
        [Display(Name = "Procurar imagens")]
        public HttpPostedFileBase[] Files { get; set; }

    }

}