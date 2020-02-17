using Kros.KORM;
using Kros.KORM.Metadata;
using Kros.Organizations.Api.Domain;

namespace Kros.Organizations.Api.Infrastructure
{
    /// <summary>
    /// Configure database for KORM.
    /// </summary>
    public class DatabaseConfiguration : DatabaseConfigurationBase
    {
        /// <summary>
        /// Create database model.
        /// </summary>
        /// <param name="modelBuilder"><c>ModelConfigurationBuilder</c></param>
        public override void OnModelCreating(ModelConfigurationBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
                .HasTableName("Organizations")
                .HasPrimaryKey(f => f.Id).AutoIncrement();
        }
    }
}
