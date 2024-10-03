﻿using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class MenuMap : AttendanceSystemEntityTypeConfiguration<Menu>
    {
        public override void Map(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(pr => new { pr.ID });
        }
    }
}