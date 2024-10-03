using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class DepartmentMap : AttendanceSystemEntityTypeConfiguration<Department>
    {
        public override void Map(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(pr => new { pr.DepartmentID });
        }
    }
}
