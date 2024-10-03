using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
   public class FixedRosterMap : AttendanceSystemEntityTypeConfiguration<FixedRoster>
    {
        public override void Map(EntityTypeBuilder<FixedRoster> builder)
        {
            builder.HasKey(pr => new { pr.FixedRosterID });
        }
    }
}
