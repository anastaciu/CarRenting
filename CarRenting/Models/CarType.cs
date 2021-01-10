
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
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
        [UniqueType(ErrorMessage = "Já existe uma categoria com o mesmo nome")]
        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Check> Checks { get; set; }


    }

}