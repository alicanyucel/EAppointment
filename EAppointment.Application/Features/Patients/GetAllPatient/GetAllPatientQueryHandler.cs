using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace EAppointment.Application.Features.Patients.GetAllPatient;

internal sealed class GetAllPatientQueryHandler(IPatientRepository patientRepository) : IRequestHandler<GetAllPatientQuery, Result<List<Patient>>>
{
    public async Task<Result<List<Patient>>> Handle(GetAllPatientQuery request, CancellationToken cancellationToken)
    {
        List<Patient> patients = await patientRepository.GetAll().OrderBy(p => p.FirstName).ToListAsync(cancellationToken);
        return patients;
    }
}
                     