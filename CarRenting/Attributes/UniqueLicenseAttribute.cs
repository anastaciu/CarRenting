using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueLicenseAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
           
                using (var dbContext = new ApplicationDbContext())
                {
                    var cars = dbContext.Cars.ToList();
                    foreach (var car in cars)
                    {
                        if (car.License == value.ToString())
                        {
                            return false;
                        }
                    }
                }
            
            return true;
        }

    }
}