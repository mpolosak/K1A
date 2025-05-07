namespace K1A.DTOs;

public class AppointmentDTO
{
    public DateTime Date { get; set; }
    public PatientDTO Patient { get; set; }
    public DoctorDTO Doctor { get; set; }
    public IEnumerable<AppointmentServiceDTO> AppointmentServices { get; set; }
}