using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
   public class WeekDayMap : AttendanceSystemEntityTypeConfiguration<WeekDays>
    {
        public override void Map(EntityTypeBuilder<WeekDays> builder)
        {
            builder.HasKey(pr => new { pr.ID });
        }
    }
}