using Hospital.Domain.Enums;
namespace Hospital.Domain.Models;

public class Treatment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SurgeonName { get; set; } = string.Empty;
    public string NurseName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TreatmentStatus Status { get; set; }
}
