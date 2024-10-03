using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class CompanyProfileMap : AttendanceSystemEntityTypeConfiguration<CompanyProfile>
    {
        public override void Map(EntityTypeBuilder<CompanyProfile> builder)
        {
            builder.HasKey(pr => new { pr.CompanyProfileID });
        }
    }
}
