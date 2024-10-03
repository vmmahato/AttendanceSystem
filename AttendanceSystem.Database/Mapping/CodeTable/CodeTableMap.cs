using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class CodeTableMap : AttendanceSystemEntityTypeConfiguration<CodeTable>
    {
        public override void Map(EntityTypeBuilder<CodeTable> builder)
        {
            builder.HasKey(pr => new { pr.ID, pr.TableName });
        }
    }
}
