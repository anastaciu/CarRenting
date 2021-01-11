using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var carTypes = dbContext.CarTypes.ToList();
                foreach (var carType in carTypes)
                {
                    if (carType.Type.ToString() == value.ToString())
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}







