using EAppointment.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Patients.GetAllPatient;
public sealed record GetAllPatientQuery():IRequest<Result<List<Patient>>>;

  
