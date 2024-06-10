using AutoMapper;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.UpdateDoctor;

internal sealed class UpdateDoctorCommandHandler(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdataDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdataDoctorCommand request, CancellationToken cancellationToken)
    {
        Doctor? doctor = await doctorRepository.GetByExpressionWithTrackingAsync(P => P.Id == request.Id, cancellationToken);
        if(doctor == null)
        {
            return Result<string>.Failure("doctor not fpund");
        }
        mapper.Map(request, doctor);
        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Doctor update is successful";

    }
}