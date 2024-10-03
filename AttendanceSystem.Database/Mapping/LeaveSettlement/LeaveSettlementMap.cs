using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class LeaveSettlementMap : AttendanceSystemEntityTypeConfiguration<LeaveSettlement>
    {
        public override void Map(EntityTypeBuilder<LeaveSettlement> builder)
        {
            builder.HasKey(pr => new { pr.SettlementID });
        }
    }
}
