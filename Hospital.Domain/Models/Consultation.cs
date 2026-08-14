using Hospital.Domain.Enums;
namespace Hospital.Domain.Models;

public class Consultation
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int TreatmentId { get; set; }
    public DateTime StartTime { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string? MedicalReport { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Planned;
}
