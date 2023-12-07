using System.ComponentModel.DataAnnotations;
namespace webApi.Models
{
    public class EmployeeDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public IFormFile Image { get; set; }
    }
}
