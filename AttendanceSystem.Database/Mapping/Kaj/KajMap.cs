using AttendanceSystem.Domains;
using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Mapping
{
   public class KajMap : AttendanceSystemEntityTypeConfiguration<Kaj>
    {
        public override void Map(EntityTypeBuilder<Kaj> builder)
        {
            builder.HasKey(pr => new { pr.KajID });
        }
    }
}
