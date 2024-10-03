using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
    public class DynamicRosterMap : AttendanceSystemEntityTypeConfiguration<DynamicRoster>
    {
        public override void Map(EntityTypeBuilder<DynamicRoster> builder)
        {
            builder.HasKey(pr => new { pr.DynamicRosterID });
        }
    }
}
