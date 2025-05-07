using System.ComponentModel.DataAnnotations;

namespace K1A.DTOs;

public class PushAppointmentDTO
{
    [Required]
    public int AppointmentId { get; set; }
    [Required]
    public int PatientId { get; set; }
    [Required]
    public string? Pwz { get; set; }
    [Required]
    public List<AppointmentServiceDTO>? Services { get; set; }
}