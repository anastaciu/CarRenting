using System;
using System.ComponentModel.DataAnnotations;

namespace CarRenting.Attributes
{
    public class MinDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime date = Convert.ToDateTime(value);
                return date >= DateTime.Now;
            }
            return false;
        }
    }
}