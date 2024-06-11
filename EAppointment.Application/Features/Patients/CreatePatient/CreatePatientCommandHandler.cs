using AutoMapper;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Patients.CreatePatient;

internal sealed class CreatePatientCommandHandler(IMapper mapper, IPatientRepository patientRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
       if(patientRepository.Any(p=>p.IdentityNumber==request.IdentityNumber))
        {
            return Result<string>.Failure("patiet already recorded");
        }
        Patient patient = mapper.Map<Patient>(request);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "patient succes added.";

    }
}
