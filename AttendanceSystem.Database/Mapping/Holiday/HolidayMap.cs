using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class HolidayMap : AttendanceSystemEntityTypeConfiguration<Holiday>
    {
        public override void Map(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasKey(pr => new { pr.HolidayID });
        }
    }
}
