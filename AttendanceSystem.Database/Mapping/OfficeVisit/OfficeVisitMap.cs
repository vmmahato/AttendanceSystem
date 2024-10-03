using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class OfficeVisitMap : AttendanceSystemEntityTypeConfiguration<OfficeVisit>
    {
        public override void Map(EntityTypeBuilder<OfficeVisit> builder)
        {
            builder.HasKey(pr => new { pr.OfficeVisitID });
        }
    }
}
