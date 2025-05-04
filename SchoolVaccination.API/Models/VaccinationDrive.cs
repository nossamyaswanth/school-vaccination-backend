using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolVaccination.API.Models
{
    public class VaccinationDrive
    {
        public int Id { get; set; }
        public string VaccineName { get; set; }
        public string Location { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}