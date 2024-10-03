using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
    public class LeaveOpeningBalanceMap : AttendanceSystemEntityTypeConfiguration<LeaveOpeningBalance>
    {
        public override void Map(EntityTypeBuilder<LeaveOpeningBalance> builder)
        {
            builder.HasKey(pr => new { pr.LeaveOpeningBalanceID });
        }
    }
}
