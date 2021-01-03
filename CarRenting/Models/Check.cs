using System;
using System.Collections.Generic;
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
        public string Description { get; set; }

        public virtual ICollection<CarType> CarTypes { get; set; }


    }
}