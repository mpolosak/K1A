using K1A.DTOs;
using K1A.Exceptions;
using Microsoft.Data.SqlClient;

namespace K1A.Service;

public class AppointmentService(IConfiguration _configuration) : IAppointmentsService
{
    private readonly string _connectionString = _configuration.GetConnectionString("DefaultConnection");
    public async Task<AppointmentDTO> GetAppointmentAsync(int id)
    {
        const string cmdText = @"SELECT date, Patient.first_name, Patient.last_name, Patient.date_of_birth, Doctor.doctor_id, Doctor.pwz,
            Service.name, Appointment_Service.service_fee FROM Appointment
            JOIN Patient ON Appointment.patient_id = Patient.patient_id
            JOIN Doctor ON Appointment.doctor_id = Doctor.doctor_id
            JOIN Appointment_Service ON Appointment.appoitment_id = Appointment_Service.appoitment_id
            JOIN Service ON Appointment_Service.service_id = Service.service_id WHERE Appointment.appoitment_id=@id;";
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(cmdText, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();
        AppointmentDTO? appointment = null;
        while (await reader.ReadAsync())
        {
            appointment ??= new AppointmentDTO()
            {
                Date = reader.GetDateTime(0),
                Patient = new PatientDTO()
                {
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    DateOfBirth = reader.GetDateTime(3),
                },
                Doctor = new DoctorDTO()
                {
                    DoctorId = reader.GetInt32(4),
                    Pwz = reader.GetString(5),
                },
                AppointmentServices = new List<AppointmentServiceDTO>(),
            };
            appointment.AppointmentServices.Add(new AppointmentServiceDTO()
            {
                Name = reader.GetString(6),
                ServiceFee = reader.GetDecimal(7),
            });
        }
        if (appointment == null) throw new NotFoundException("Appointment not found");
        return appointment;
    }

    public async Task AddAppointmentAsync(PushAppointmentDTO appointment)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        var transaction = conn.BeginTransaction();
        const string cmdText = "";
        await using var cmd = new SqlCommand(cmdText, conn, transaction);
        try
        {
            var doctorId = 0; // TODO: GetDoctorId
            cmd.Parameters.Clear();
            cmd.CommandText = @"INSERT INTO Appointment VALUES (@AppointmentId, @PatientId, @DoctorId,@Date)";
            cmd.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
            cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
            cmd.Parameters.AddWithValue("@DoctorId", doctorId);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw new ConflictException("Appointment already exists");
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}