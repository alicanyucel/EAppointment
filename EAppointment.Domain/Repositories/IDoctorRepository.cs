using EAppointment.Domain.Entities;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAppointment.Domain.Repositories
{
    public interface IDoctorRepository:IRepository<Doctor>
    {
    }
}
