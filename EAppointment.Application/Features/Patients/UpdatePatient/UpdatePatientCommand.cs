using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Patients.UpdatePatient;
public sealed record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string IdentityNumber,
    string City,
    string Town,
    string FullAdress) : IRequest<Result<string>>;
