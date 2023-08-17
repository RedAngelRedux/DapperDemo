using System.ComponentModel.DataAnnotations;

namespace DapperDemoWebApp.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public List<Employee> Employees { get; set; }

    }
}
