using Kros.KORM;
using Kros.KORM.Metadata;
using Kros.Tags.Api.Domain;

namespace Kros.Tags.Api.Infrastructure
{
    public class DatabaseConfiguration : DatabaseConfigurationBase
    {
        internal const string TagsTableName = "Tags";

        /// <summary>
        /// Create database model.
        /// </summary>
        /// <param name="modelBuilder"><c>ModelConfigurationBuilder</c></param>
        public override void OnModelCreating(ModelConfigurationBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasTableName(TagsTableName)
                .HasPrimaryKey(f => f.Id).AutoIncrement();
        }
    }
}
