using EAppointment.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace EAppointment.Application.Features.Users.GetAllRolesForUsers;
public sealed record GetAllRolesForUsersQuery() : IRequest<Result<List<AppRole>>>;