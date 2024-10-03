using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class RolesMap : AttendanceSystemEntityTypeConfiguration<Roles>
    {
        public override void Map(EntityTypeBuilder<Roles> builder)
        {
            builder.HasKey(pr => new { pr.ID });
        }
    }
}
