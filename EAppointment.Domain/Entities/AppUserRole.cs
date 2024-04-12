using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAppointment.Domain.Entities
{
    public sealed  class AppUserRole:IdentityUserRole<Guid>
    {
    }
}
