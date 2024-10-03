using Database;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Mapping
{
    public class LeaveQuotaMap : AttendanceSystemEntityTypeConfiguration<LeaveQuota>
    {
        public override void Map(EntityTypeBuilder<LeaveQuota> builder)
        {
            builder.HasKey(pr => new { pr.LeaveQuotaID });
        }
    }
}
