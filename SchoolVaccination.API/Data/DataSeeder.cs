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
                    new Student { FullName = "Manish Yadav", Grade = "7th", DateOfBirth = new DateTime(2010, 9, 30), CertificateFileName = "student_3_certificate.pdf" },
                    new Student { FullName = "John Doe", Grade = "8th", DateOfBirth = new DateTime(2010, 5, 15), CertificateFileName = null },
                    new Student { FullName = "Alice Johnson", Grade = "9th", DateOfBirth = new DateTime(2009, 8, 20), CertificateFileName = null }
                );
                context.SaveChanges();
            }

            if (!context.VaccinationDrives.Any())
            {
                context.VaccinationDrives.AddRange(
                    new VaccinationDrive { VaccineName = "Polio", Location = "School Clinic", ScheduledDate = new DateTime(2025, 5, 10) },
                    new VaccinationDrive { VaccineName = "Measles", Location = "Community Center", ScheduledDate = new DateTime(2025, 5, 12) }
                );
                context.SaveChanges();
            }

            if (!context.VaccinationRecords.Any())
            {
                var students = context.Students.ToList();
                var drive1 = context.VaccinationDrives.FirstOrDefault(d => d.VaccineName == "Polio");
                var drive2 = context.VaccinationDrives.FirstOrDefault(d => d.VaccineName == "Measles");

                foreach (var student in students)
                {
                    if (student.FullName == "Ravi Kumar" && drive1 != null)
                    {
                        context.VaccinationRecords.Add(new VaccinationRecord
                        {
                            StudentId = student.Id,
                            DriveId = drive1.Id,
                            IsVaccinated = true,
                            Date = drive1.ScheduledDate
                        });
                    }

                    if (student.FullName == "Anjali Sharma" && drive1 != null)
                    {
                        context.VaccinationRecords.Add(new VaccinationRecord
                        {
                            StudentId = student.Id,
                            DriveId = drive1.Id,
                            IsVaccinated = false,
                            Date = drive1.ScheduledDate
                        });
                    }

                    if (student.FullName == "Manish Yadav" && drive2 != null)
                    {
                        context.VaccinationRecords.Add(new VaccinationRecord
                        {
                            StudentId = student.Id,
                            DriveId = drive2.Id,
                            IsVaccinated = true,
                            Date = drive2.ScheduledDate
                        });
                    }

                    // Add default records for students without vaccination records
                    if (student.FullName == "John Doe" || student.FullName == "Alice Johnson")
                    {
                        context.VaccinationRecords.Add(new VaccinationRecord
                        {
                            StudentId = student.Id,
                            DriveId = drive1?.Id ?? 0, // Use a valid drive ID or 0 if no drive exists
                            IsVaccinated = false,
                            Date = drive1?.ScheduledDate ?? DateTime.MinValue
                        });
                    }
                }

                context.SaveChanges();
            }

            // Synchronize IsVaccinated field in Students table
            var allStudents = context.Students.ToList();
            foreach (var student in allStudents)
            {
                student.IsVaccinated = context.VaccinationRecords
                    .Any(v => v.StudentId == student.Id && v.IsVaccinated);
            }
            context.SaveChanges();
        }
    }
}