using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
  public  class WorkShiftTypeMap : AttendanceSystemEntityTypeConfiguration<WorkShiftType>
    {
        public override void Map(EntityTypeBuilder<WorkShiftType> builder)
        {
            builder.HasKey(pr => new { pr.ShiftTypeID });
        }
    }
}
