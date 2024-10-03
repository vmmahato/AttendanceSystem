using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
  public  class WorkShiftMap : AttendanceSystemEntityTypeConfiguration<WorkShift>
    {
        public override void Map(EntityTypeBuilder<WorkShift> builder)
        {
            builder.HasKey(pr => new { pr.ShiftID });
        }
    }
}