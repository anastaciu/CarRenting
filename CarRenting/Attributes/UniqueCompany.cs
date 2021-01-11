using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueCompany : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var companies = dbContext.Companies.ToList();
                    foreach (var company in companies)
                    {
                        if (company.CompanyName == value.ToString())
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