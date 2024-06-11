using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Patients.CreatePatient;

internal sealed record CreatePatientCommand(string FirstName,string LastName,string FullAddress,string Town,string City,string IdentityNumber):IRequest<Result<string>>;
