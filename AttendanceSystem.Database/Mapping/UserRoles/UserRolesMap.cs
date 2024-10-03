using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class UserRolesMap : AttendanceSystemEntityTypeConfiguration<UserRoles>
    {
        public override void Map(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasKey(pr =>new { pr.UserRolesID });
        }
    }
}
