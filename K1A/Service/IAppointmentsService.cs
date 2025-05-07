using K1A.DTOs;

namespace K1A.Service;

public interface IAppointmentsService
{
    public Task<AppointmentDTO> GetAppointmentAsync(int id);
    public Task AddAppointmentAsync(PushAppointmentDTO appointment);
}