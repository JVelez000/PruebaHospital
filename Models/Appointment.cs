using System.ComponentModel.DataAnnotations;

namespace PruebaHospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El médico es requerido")]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Required(ErrorMessage = "El paciente es requerido")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "La hora es requerida")]
        public TimeSpan Time { get; set; }

        public string Reason { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pendiente;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        
        public bool EmailSent { get; set; } = false;
        public DateTime? EmailSentAt { get; set; }
        public string? EmailStatus { get; set; }
        
        public DateTime FullDateTime => Date.Add(Time);
        
        public string FormattedDate => Date.ToString("dd/MM/yyyy");
        public string FormattedTime => Time.ToString(@"hh\:mm");
        public string FormattedDateTime => $"{FormattedDate} {FormattedTime}";
        
        public string StatusDisplay => Status.ToString();
        public string StatusBadgeClass => Status switch
        {
            AppointmentStatus.Pendiente => "bg-warning",
            AppointmentStatus.Confirmada => "bg-primary", 
            AppointmentStatus.Atendida => "bg-success",
            AppointmentStatus.Cancelada => "bg-danger",
            AppointmentStatus.NoAsistio => "bg-secondary",
            _ => "bg-light"
        };
    }
}