using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
   public class DeviceModelMap : AttendanceSystemEntityTypeConfiguration<DeviceModel>
    {
        public override void Map(EntityTypeBuilder<DeviceModel> builder)
        {
            builder.HasKey(pr => new { pr.DeviceModelID });
        }
    }
}