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
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "admin", Password = "admin123", Role = "Admin" },
                    new User { Username = "coordinator", Password = "pass123", Role = "User" }
                );
                context.SaveChanges();
            }

            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { FullName = "Ravi Kumar", Grade = "5th", DateOfBirth = new DateTime(2012, 6, 15), CertificateFileName = "student_1_certificate.pdf" },
                    new Student { FullName = "Anjali Sharma", Grade = "6th", DateOfBirth = new DateTime(2011, 3, 22), CertificateFileName = null },
                    new Student { FullName = "Manish Yadav", Grade = "7th", DateOfBirth = new DateTime(2010, 9, 30), CertificateFileName = "student_3_certificate.pdf" }
                );
                context.SaveChanges();
            }

            if (!context.VaccinationDrives.Any())
            {
                context.VaccinationDrives.AddRange(
                    new VaccinationDrive { VaccineName = "Polio", Location = "Govt High School", ScheduledDate = new DateTime(2025, 5, 10) },
                    new VaccinationDrive { VaccineName = "Measles", Location = "Green Valley School", ScheduledDate = new DateTime(2025, 5, 12) }
                );
                context.SaveChanges();
            }

            if (!context.VaccinationRecords.Any())
            {
                var student1 = context.Students.FirstOrDefault(s => s.FullName == "Ravi Kumar");
                var student2 = context.Students.FirstOrDefault(s => s.FullName == "Anjali Sharma");
                var drive1 = context.VaccinationDrives.FirstOrDefault(d => d.VaccineName == "Polio");
                var drive2 = context.VaccinationDrives.FirstOrDefault(d => d.VaccineName == "Measles");

                if (student1 != null && drive1 != null)
                {
                    context.VaccinationRecords.Add(new VaccinationRecord
                    {
                        StudentId = student1.Id,
                        DriveId = drive1.Id,
                        IsVaccinated = true,
                        Date = drive1.ScheduledDate
                    });
                }

                if (student2 != null && drive1 != null)
                {
                    context.VaccinationRecords.Add(new VaccinationRecord
                    {
                        StudentId = student2.Id,
                        DriveId = drive1.Id,
                        IsVaccinated = false,
                        Date = drive1.ScheduledDate
                    });
                }

                if (student1 != null && drive2 != null)
                {
                    context.VaccinationRecords.Add(new VaccinationRecord
                    {
                        StudentId = student1.Id,
                        DriveId = drive2.Id,
                        IsVaccinated = true,
                        Date = drive2.ScheduledDate
                    });
                }

                context.SaveChanges();
            }

            // Synchronize IsVaccinated field in Students table
            var students = context.Students.ToList();
            foreach (var student in students)
            {
                student.IsVaccinated = context.VaccinationRecords
                    .Any(v => v.StudentId == student.Id && v.IsVaccinated);
            }
            context.SaveChanges();
        }
    }
}