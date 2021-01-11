using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRenting.Attributes
{
    public class MinDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime date = Convert.ToDateTime(value);
                return date.Day >= DateTime.Now.Day;
            }
            return false;
        }
    }
}