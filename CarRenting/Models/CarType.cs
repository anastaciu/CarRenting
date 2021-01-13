using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarRenting.Attributes;

namespace CarRenting.Models
{

    public class CarType
    {
        public CarType()
        {
            this.Checks = new HashSet<Check>();
        }
        public int Id { get; set; }
        [Display(Name = "Categoria")]
        [Required]
        [UniqueType(ErrorMessage = "Já existe uma categoria com o mesmo nome")]
        public string Type { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Check> Checks { get; set; }


    }

}