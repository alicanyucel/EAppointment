using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using EAppointment.Infrastructure.Context;
using GenericRepository;

namespace EAppointment.Infrastructure.Repositories;

internal sealed class UserRoleRepository : Repository<AppUserRole, ApplicationDbContext>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
