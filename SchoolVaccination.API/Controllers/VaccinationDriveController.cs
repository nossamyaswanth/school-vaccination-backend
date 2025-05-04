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
    public class VaccinationDriveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VaccinationDriveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/vaccinationdrive
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaccinationDrive>>> GetDrives()
        {
            return await _context.VaccinationDrives.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<VaccinationDrive>> PostDrive(VaccinationDrive drive)
        {
            var today = DateTime.Today;
            if ((drive.ScheduledDate - today).TotalDays < 15)
            {
                return BadRequest("Drives must be scheduled at least 15 days in advance.");
            }

            // Prevent overlapping drives (same date and same class if applicable later)
            bool conflictExists = await _context.VaccinationDrives
                .AnyAsync(d => d.ScheduledDate.Date == drive.ScheduledDate.Date);

            if (conflictExists)
            {
                return BadRequest("A drive is already scheduled on this date. Please choose another date.");
            }

            _context.VaccinationDrives.Add(drive);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDrives), new { id = drive.Id }, drive);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDrive(int id, VaccinationDrive updatedDrive)
        {
            var existingDrive = await _context.VaccinationDrives.FindAsync(id);
            if (existingDrive == null) return NotFound();

            if (existingDrive.ScheduledDate < DateTime.Today)
            {
                return BadRequest("Cannot edit a past drive.");
            }

            // Optional: re-validate new date
            if ((updatedDrive.ScheduledDate - DateTime.Today).TotalDays < 15)
            {
                return BadRequest("Updated drive must still be at least 15 days in advance.");
            }

            // Update fields
            existingDrive.VaccineName = updatedDrive.VaccineName;
            existingDrive.ScheduledDate = updatedDrive.ScheduledDate;
            existingDrive.Location = updatedDrive.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}