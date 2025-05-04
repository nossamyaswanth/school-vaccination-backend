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
    public class VaccinationRecordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VaccinationRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateRecord([FromBody] VaccinationRecord record)
        {
            var existing = await _context.VaccinationRecords
                .FirstOrDefaultAsync(r => r.StudentId == record.StudentId && r.DriveId == record.DriveId);

            if (existing != null)
            {
                // Update
                existing.IsVaccinated = record.IsVaccinated;
                existing.Date = record.Date;
            }
            else
            {
                // Create
                _context.VaccinationRecords.Add(record);
            }

            await _context.SaveChangesAsync();
            return Ok(record);
        }
    }
}