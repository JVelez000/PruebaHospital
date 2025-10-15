namespace PruebaHospital.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Document { get; set; } = string.Empty; // ← NUEVO CAMPO
        public string Name { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        public List<Appointment> Appointments { get; set; } = new();
        
        public string FullInfo => $"{Name} - {Speciality}"; // ← PROPIEADAD ÚTIL
    }
}