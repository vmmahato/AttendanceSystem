using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class LeaveSetupMap : AttendanceSystemEntityTypeConfiguration<LeaveSetup>
    {
        public override void Map(EntityTypeBuilder<LeaveSetup> builder)
        {
            builder.HasKey(pr => new { pr.LeaveID });
        }
    }
    public class DefaultLeaveMap : AttendanceSystemEntityTypeConfiguration<DefaultLeave>
    {
        public override void Map(EntityTypeBuilder<DefaultLeave> builder)
        {
            builder.HasKey(pr => new { pr.LeaveID });
        }
    }
}
