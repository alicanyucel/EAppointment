using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using EAppointment.Infrastructure.Context;
using GenericRepository;

namespace EAppointment.Infrastructure.Repositories
{
    internal sealed class PatientRepository : Repository<Patient, ApplicationDbContext>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
