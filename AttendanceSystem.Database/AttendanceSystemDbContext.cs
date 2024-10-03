using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AttendanceSystem.Database
{
  public class AttendanceSystemDbContext: DbContext
    {
        public AttendanceSystemDbContext(DbContextOptions<AttendanceSystemDbContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all configuration

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(AttendanceSystemEntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                // 1. Create instance of Mapper Class
                var mapperInstance = Activator.CreateInstance(type);
                // 2. Get the Generic type T of the Mapper Class ie: CodeTable from CodeTableMap<CodeTable>
                Type itemType = type.BaseType.GetGenericArguments()[0];
                // 3. Call the Entity Method from builder to get EntityTypeConfiguration<T>
                // note: Entity method is overloaded. new Type[0] added to take the method without parameter.
                var mapBuilder = modelBuilder.GetType().GetMethod("Entity", new Type[0]).MakeGenericMethod(itemType).Invoke(modelBuilder, null);
                // 4. Call the Map function in Mapper instance with required parameters.
                mapperInstance.GetType().GetMethod("Map").Invoke(mapperInstance, new object[] { mapBuilder });
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
