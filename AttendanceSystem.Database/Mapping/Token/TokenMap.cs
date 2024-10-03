using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class TokenMap : AttendanceSystemEntityTypeConfiguration<Token>
    {
        public override void Map(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(pr => new { pr.TokenID,pr.UserID });
        }
    }
}
