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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadCertificate(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"student_{id}_certificate{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            student.CertificateFileName = fileName;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "File uploaded successfully", FileName = fileName });
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null || string.IsNullOrEmpty(student.CertificateFileName))
                return NotFound("Certificate not found.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Certificates");
            var filePath = Path.Combine(folderPath, student.CertificateFileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found on server.");

            var contentType = "application/octet-stream";
            return File(System.IO.File.ReadAllBytes(filePath), contentType, student.CertificateFileName);
        }
    }
}