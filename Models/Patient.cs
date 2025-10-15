using System.ComponentModel.DataAnnotations;

namespace PruebaHospital.Models
{
    public class Patient
    {
        public int Id { get; set; }
        
        [Required]
        public string Document { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        public List<Appointment> Appointments { get; set; } = new();
        
        public string FullName => $"{FirstName} {LastName}";
    }
}