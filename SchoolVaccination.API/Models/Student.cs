using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolVaccination.API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsVaccinated { get; set; }
        public string? CertificateFileName { get; set; }

        // Navigation property (optional)
    public ICollection<VaccinationRecord>? VaccinationRecords { get; set; }
    }
}