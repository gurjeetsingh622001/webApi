using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.IO.Pipes;
using webApi.Models;
using webApi.NewFolder;
using static System.Net.Mime.MediaTypeNames;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private ApplicationContext context;
        private readonly IWebHostEnvironment environment;
        public EmployeeController(ApplicationContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;

        }
        [HttpGet(Name = "GetEmployee")]
        public async Task<ActionResult<List<Employee>>> getEmployees()
        {
            try
            {
                var result = await context.Employees.ToListAsync();
                return Ok(result);
            }
            catch (Exception er)
            {
                throw er;
            }
        }
        [HttpPost(Name = "AddEmployee")]
        public async Task<ActionResult> addEmployee([FromForm] EmployeeDto employee)
        {
            try
            {
                string filename = "";
                var extension = "." + employee.Image.FileName.Split('.')[employee.Image.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var exactPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactPath, FileMode.Create))
                {
                    await employee.Image.CopyToAsync(stream);
                }

                var Employee = new Employee()
                {
                    Name = employee.Name,
                    Gender = employee.Gender,
                    Email = employee.Email,
                    Country = employee.Country,
                    ImageName = filename

                };
                context.Employees.AddAsync(Employee);
                context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception er)
            {
                return StatusCode(500, "An error occurred: " + er.Message);
            }
        }

        private void UploadFile_(IFormFile file, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
        }

        private void UploadFile(IFormFile file, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
            }
        }
    }
}
