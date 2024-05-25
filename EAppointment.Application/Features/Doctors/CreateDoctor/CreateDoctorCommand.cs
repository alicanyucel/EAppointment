using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.CreateDoctor
{
    public sealed  record  CreateDoctorCommand(string FirstName,string LastName,int Departmrnt):IRequest<Result<string>>;

}
