using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueCompanyEmailAttribute : ValidationAttribute
    {
        public class UniqueCompany : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var companies = dbContext.Companies.ToList();
                    foreach (var company in companies)
                    {
                        if (company.Email == value.ToString())
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}