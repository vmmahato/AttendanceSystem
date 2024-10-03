using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class DeviceMap : AttendanceSystemEntityTypeConfiguration<Device>
    {
        public override void Map(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(pr => new { pr.DeviceID });
        }
    }
}
