using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolVaccination.API.Data;
using SchoolVaccination.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace SchoolVaccination.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public IActionResult GetVaccinationSummary()
        {
            var totalStudents = _context.Students.Count();

            var vaccinatedCount = _context.VaccinationRecords
                .Where(v => v.IsVaccinated)
                .Select(v => v.StudentId)
                .Distinct()
                .Count();

            var notVaccinated = totalStudents - vaccinatedCount;

            var driveSummary = _context.VaccinationDrives
                .Select(d => new
                {
                    d.VaccineName,
                    d.ScheduledDate,
                    TotalRegistered = _context.VaccinationRecords.Count(v => v.DriveId == d.Id),
                    Vaccinated = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && v.IsVaccinated),
                    Missed = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && !v.IsVaccinated)
                }).ToList();

            return Ok(new
            {
                TotalStudents = totalStudents,
                Vaccinated = vaccinatedCount,
                NotVaccinated = notVaccinated,
                DriveSummary = driveSummary
            });
        }
    }
}