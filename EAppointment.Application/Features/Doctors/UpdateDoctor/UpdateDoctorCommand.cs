using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.UpdateDoctor;

public sealed record UpdataDoctorCommand(Guid Id,string FirstName,string LastName,int DepartmentValue):IRequest<Result<string>>;
