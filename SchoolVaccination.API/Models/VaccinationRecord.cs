using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolVaccination.API.Models
{
    public class VaccinationRecord
    {
        public int Id { get; set; }

        // Foreign keys
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int DriveId { get; set; }
        public VaccinationDrive? Drive { get; set; }

        // Status and Date
        public bool IsVaccinated { get; set; }
        public DateTime Date { get; set; }
    }
}