﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAppointment.Domain.Entities
{
    public sealed class Doctor
    {
        public Doctor()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName=>string.Join(" ", FirstName, LastName);
        public DepartmentEnum Department { get; set; }=DepartmentEnum.Acil;
        
    }
}