using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
    public class DeviceLogsMap : AttendanceSystemEntityTypeConfiguration<DeviceLogs>
    {
        public override void Map(EntityTypeBuilder<DeviceLogs> builder)
        {
            builder.HasKey(pr => new { pr.DeviceLogsID });
        }
    }
}