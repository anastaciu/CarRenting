using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRenting.Models
{
    public class Check
    {

        public Check()
        {
            this.CarTypes = new HashSet<CarType>();
        }
        public int Id { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "A descrição da Verificação é necessária")]
        public string Description { get; set; }
        public virtual ICollection<CarType> CarTypes { get; set; }
        [ForeignKey("Company")]
        public virtual int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}