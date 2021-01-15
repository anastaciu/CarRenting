using System.Collections.Generic;
using System.Web.Mvc;

namespace CarRenting.Utils
{
    public class SelectLists
    {
        public static List<SelectListItem> FuelLevelList()
        {
            List<SelectListItem> fuelList = new List<SelectListItem>
            {
                new SelectListItem { Value = @"Vazio", Text = @"Vazio" },
                new SelectListItem { Value = @"Meio Vazio", Text = @"Meio Vazio"},
                new SelectListItem { Value = @"Meio", Text = @"Meio" },
                new SelectListItem { Value = @"Meio Cheio", Text = @"Meio Cheio"},
                new SelectListItem { Value = "Cheio", Text = @"Cheio" }
            };
            return fuelList;
        }
    }
}