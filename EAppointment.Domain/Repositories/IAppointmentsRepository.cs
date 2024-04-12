using EAppointment.Domain.Entities;
using GenericRepository;

namespace EAppointment.Domain.Repositories
{
    public interface IAppointmentsRepository:IRepository<Appointment> { }

}
