using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using EAppointment.Infrastructure.Context;
using GenericRepository;

namespace EAppointment.Infrastructure.Repositories
{
    internal sealed class DoctorRepository : Repository<Doctor, ApplicationDbContext>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
