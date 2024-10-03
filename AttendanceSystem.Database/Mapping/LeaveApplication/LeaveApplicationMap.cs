using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class LeaveApplicationMap : AttendanceSystemEntityTypeConfiguration<LeaveApplication>
    {
        public override void Map(EntityTypeBuilder<LeaveApplication> builder)
        {
            builder.HasKey(pr => new { pr.LeaveApplicationID });
        }
    }
}
