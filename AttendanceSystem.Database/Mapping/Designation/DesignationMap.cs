using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class DesignationMap : AttendanceSystemEntityTypeConfiguration<Designation>
    {
        public override void Map(EntityTypeBuilder<Designation> builder)
        {
            builder.HasKey(pr => new { pr.DesignationID });
        }
    }
}
