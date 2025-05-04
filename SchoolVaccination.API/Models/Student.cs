using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolVaccination.API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Grade { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsVaccinated { get; set; }
        public string? CertificateFileName { get; set; }

        // Navigation property (optional)
    public ICollection<VaccinationRecord>? VaccinationRecords { get; set; }
    }
}