﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueTypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
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
                }
            }
            return true;
        }
    }
}







