namespace Hospital.Domain.Models
{
	public class PatientDocument
	{
		public int Id { get; set; }

		public int PatientId { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string ContentType { get; set; } = string.Empty;

		public string FilePath { get; set; } = string.Empty;

		public DateTime UploadedAt { get; set; }

		public string UploadedBy { get; set; } = string.Empty;

		public bool UploadedByHealthcareProvider { get; set; }
	}
}