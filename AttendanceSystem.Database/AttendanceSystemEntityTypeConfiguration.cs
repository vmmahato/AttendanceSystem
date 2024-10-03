using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public interface IAttendanceSystemEntityTypeConfiguration<TEntityType> where TEntityType : class
    {
        void Map(EntityTypeBuilder<TEntityType> builder);
    }

    public abstract class AttendanceSystemEntityTypeConfiguration<TEntityType> : IAttendanceSystemEntityTypeConfiguration<TEntityType>
        where TEntityType : BaseEntity
    {
        public abstract void Map(EntityTypeBuilder<TEntityType> builder);
    }
}
