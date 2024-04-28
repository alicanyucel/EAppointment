using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using EAppointment.Infrastructure.Context;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAppointment.Infrastructure.Repositories
{
    internal sealed class AppointmentRepository:Repository<Appointment,ApplicationDbContext>,IAppointmentsRepository
    {
        public AppointmentRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}
