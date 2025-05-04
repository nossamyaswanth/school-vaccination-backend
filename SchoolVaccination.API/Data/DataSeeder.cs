using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolVaccination.API.Models;

namespace SchoolVaccination.API.Data
{
    public class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { FullName = "Ravi Kumar", Grade = "5th", DateOfBirth = new DateTime(2012, 6, 15), IsVaccinated = true },
                    new Student { FullName = "Anjali Sharma", Grade = "6th", DateOfBirth = new DateTime(2011, 3, 22), IsVaccinated = false },
                    new Student { FullName = "Manish Yadav", Grade = "7th", DateOfBirth = new DateTime(2010, 9, 30), IsVaccinated = true }
                );
                context.SaveChanges();
            }
        }
    }
}