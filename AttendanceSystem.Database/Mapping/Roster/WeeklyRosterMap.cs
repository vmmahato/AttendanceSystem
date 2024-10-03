using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
    public class WeeklyRosterMap : AttendanceSystemEntityTypeConfiguration<WeeklyRoster>
    {
        public override void Map(EntityTypeBuilder<WeeklyRoster> builder)
        {
            builder.HasKey(pr => new { pr.WeeklyRosterID });
        }
    }
}