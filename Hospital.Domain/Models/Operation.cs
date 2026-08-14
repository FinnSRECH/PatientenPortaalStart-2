using Hospital.Domain.Enums;

namespace Hospital.Domain.Models;

public class Operation
{
	public int Id { get; set; }

	public int PatientId { get; set; }

	public int TreatmentId { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public DateTime StartTime { get; set; }

	public string SurgeonName { get; set; } = string.Empty;

	public string OperatingRoom { get; set; } = string.Empty;

	public AppointmentStatus Status { get; set; }

	public string MedicalReport { get; set; } = string.Empty;
}