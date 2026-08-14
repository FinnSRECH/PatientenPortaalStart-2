using System.ComponentModel.DataAnnotations;

namespace Hospital.Domain.Models;

public class Complaint
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    [Required, StringLength(80)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsProcessed { get; set; }
}
