using System.Collections.Generic;

namespace CarRenting.Models
{
    public class AssociateViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<CheckBoxes> CarTypes { get; set; }
    }

    public class CheckBoxes
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Add { get; set; }
    }
}