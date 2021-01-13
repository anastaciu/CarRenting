using System.ComponentModel.DataAnnotations;
using System.Linq;
using CarRenting.Models;

namespace CarRenting.Attributes
{
    public class UniqueImagePathAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            using (var dbContext = new ApplicationDbContext())
            {

                var imagePaths = dbContext.DamageImages.ToList();
                foreach (var imagePath in imagePaths)
                {
                    if (imagePath.ImagePath == value.ToString())
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }

}