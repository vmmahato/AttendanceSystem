using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class SectionMap : AttendanceSystemEntityTypeConfiguration<Section>
    {
        public override void Map(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(pr => new { pr.SectionID });
        }
    }
}
