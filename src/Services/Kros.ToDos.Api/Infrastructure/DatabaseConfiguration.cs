using Kros.KORM;
using Kros.KORM.Metadata;
using Kros.ToDos.Api.Domain;

namespace Kros.ToDos.Api.Infrastructure
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
            modelBuilder.Entity<ToDo>()
                .HasTableName("ToDos")
                .HasPrimaryKey(f => f.Id).AutoIncrement();
        }
    }
}
