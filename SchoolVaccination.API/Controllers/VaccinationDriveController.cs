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

        // POST: api/vaccinationdrive
        [HttpPost]
        public async Task<ActionResult<VaccinationDrive>> PostDrive(VaccinationDrive drive)
        {
            _context.VaccinationDrives.Add(drive);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDrives), new { id = drive.Id }, drive);
        }
    }
}