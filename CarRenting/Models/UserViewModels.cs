using System.ComponentModel.DataAnnotations;

namespace CarRenting.Models
{
    public class DashViewModel
    {
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }

    }
    
    public class UserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Display(Name = "Nome")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Telefone/Telemóvel")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Permissões")]
        [Required]
        public string Role { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Empresa")]
        public string CompanyName { get; set; }
    }


}