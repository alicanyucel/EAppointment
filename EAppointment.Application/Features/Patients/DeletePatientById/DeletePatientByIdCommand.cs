using EAppointment.Application.Features.Doctors.DeleteDoctorById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Patients.DeletePatientById;

public sealed record DeletePatientByIdCommand(Guid Id):IRequest<Result<string>>;
