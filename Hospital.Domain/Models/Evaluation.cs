namespace Hospital.Domain.Models;

public class Evaluation
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int ConsultationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CareProviderName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
