using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class FiscalYearsMap : AttendanceSystemEntityTypeConfiguration<FiscalYears>
    {
        public override void Map(EntityTypeBuilder<FiscalYears> builder)
        {
            builder.HasKey(pr => new { pr.FiscalID });
        }
    }
}
