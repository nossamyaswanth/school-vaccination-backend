using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolVaccination.API.Data;
using SchoolVaccination.API.Models;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

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

        // [HttpGet("summary")]
        // public IActionResult GetVaccinationSummary()
        // {
        //     // Fetch data without recalculating IsVaccinated
        //     var totalStudents = _context.Students.Count();
        //     var vaccinatedCount = _context.Students.Count(s => s.IsVaccinated);
        //     var notVaccinated = totalStudents - vaccinatedCount;

        //     var driveSummary = _context.VaccinationDrives
        //         .Select(d => new
        //         {
        //             d.VaccineName,
        //             d.ScheduledDate,
        //             TotalRegistered = _context.VaccinationRecords.Count(v => v.DriveId == d.Id),
        //             Vaccinated = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && v.IsVaccinated),
        //             Missed = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && !v.IsVaccinated)
        //         }).ToList();

        //     return Ok(new
        //     {
        //         TotalStudents = totalStudents,
        //         Vaccinated = vaccinatedCount,
        //         NotVaccinated = notVaccinated,
        //         DriveSummary = driveSummary
        //     });
        // }

        [HttpGet("summary")]
        public IActionResult GetVaccinationSummary()
        {
            // Fetch total students and vaccination counts
            var totalStudents = _context.Students.Count();
            var vaccinatedCount = _context.Students.Count(s => s.IsVaccinated);
            var notVaccinated = totalStudents - vaccinatedCount;

            // Calculate missed vaccinations
            var missedVaccinations = totalStudents - vaccinatedCount - notVaccinated;

            // Fetch drive performance data
            var driveSummary = _context.VaccinationDrives
                .Select(d => new
                {
                    d.VaccineName,
                    d.ScheduledDate,
                    d.Location,
                    TotalRegistered = _context.VaccinationRecords.Count(v => v.DriveId == d.Id),
                    Vaccinated = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && v.IsVaccinated),
                    Missed = _context.VaccinationRecords.Count(v => v.DriveId == d.Id && !v.IsVaccinated)
                }).ToList();

            // Generate vaccination trends (grouped by date)
            var vaccinationTrends = _context.VaccinationRecords
                .GroupBy(v => v.Date.Date) // Use the correct property name
                .Select(g => new
                {
                    Date = g.Key,
                    Vaccinated = g.Count(v => v.IsVaccinated),
                    Missed = g.Count(v => !v.IsVaccinated)
                })
                .OrderBy(t => t.Date)
                .ToList();

            // Return the aggregated data
            return Ok(new
            {
                TotalStudents = totalStudents,
                Vaccinated = vaccinatedCount,
                NotVaccinated = notVaccinated,
                MissedVaccinations = missedVaccinations,
                DriveSummary = driveSummary,
                VaccinationTrends = vaccinationTrends // Ensure this is aggregated if needed
            });
        }

        [HttpGet("download-vaccination-details")]
        public IActionResult DownloadVaccinationDetails()
        {
            // Fetch all students with their vaccination details (left join)
            var vaccinationDetails = _context.Students
                .GroupJoin(
                    _context.VaccinationRecords.Include(v => v.Drive),
                    student => student.Id,
                    record => record.StudentId,
                    (student, records) => new { Student = student, Records = records.DefaultIfEmpty() }
                )
                .SelectMany(
                    studentWithRecords => studentWithRecords.Records.DefaultIfEmpty(), // Ensure null records are included
                    (studentWithRecords, record) => new
                    {
                        StudentName = studentWithRecords.Student.FullName,
                        Grade = studentWithRecords.Student.Grade,
                        DateOfBirth = studentWithRecords.Student.DateOfBirth,
                        IsVaccinated = record != null && record.IsVaccinated,
                        DateOfVaccination = record.Date,
                        VaccineName = record.Drive.VaccineName
                    }
                )
                .ToList();

            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Generate Excel file
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("VaccinationDetails");

                // Add headers
                worksheet.Cells[1, 1].Value = "Student Name";
                worksheet.Cells[1, 2].Value = "Grade";
                worksheet.Cells[1, 3].Value = "Date of Birth";
                worksheet.Cells[1, 4].Value = "Is Vaccinated";
                worksheet.Cells[1, 5].Value = "Date of Vaccination";
                worksheet.Cells[1, 6].Value = "Vaccine Name";

                // Add data
                for (int i = 0; i < vaccinationDetails.Count; i++)
                {
                    var detail = vaccinationDetails[i];
                    worksheet.Cells[i + 2, 1].Value = detail.StudentName;
                    worksheet.Cells[i + 2, 2].Value = detail.Grade;
                    worksheet.Cells[i + 2, 3].Value = detail.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 4].Value = detail.IsVaccinated ? "Yes" : "No";
                    worksheet.Cells[i + 2, 5].Value = detail.DateOfVaccination.ToString("yyyy-MM-dd") ?? string.Empty;
                    worksheet.Cells[i + 2, 6].Value = detail.VaccineName ?? string.Empty;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Return the Excel file as a downloadable response
                var excelData = package.GetAsByteArray();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VaccinationDetails.xlsx");
            }
        }
    }
}