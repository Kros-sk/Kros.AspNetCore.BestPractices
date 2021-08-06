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
        internal const string OrganizationsTableName = "Organizations";

        /// <summary>
        /// Create database model.
        /// </summary>
        /// <param name="modelBuilder"><c>ModelConfigurationBuilder</c></param>
        public override void OnModelCreating(ModelConfigurationBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
                .HasTableName(OrganizationsTableName)
                .HasPrimaryKey(f => f.Id).AutoIncrement();
        }
    }
}
