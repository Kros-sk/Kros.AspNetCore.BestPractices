using Kros.Authorization.Api.Domain;
using Kros.KORM;
using Kros.KORM.Converter;
using Kros.KORM.Metadata;

namespace Kros.Authorization.Api.Infrastructure
{
    /// <summary>
    /// Configure database for KORM.
    /// </summary>
    public class DatabaseConfiguration : DatabaseConfigurationBase
    {
        /// <summary>
        /// Name of Users table in database.
        /// </summary>
        internal const string UsersTableName = "Users";

        /// <summary>
        /// Name of Permissions table in database.
        /// </summary>
        internal const string PermissionsTableName = "Permissions";

        /// <summary>
        /// Create database model.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelConfigurationBuilder"/></param>
        public override void OnModelCreating(ModelConfigurationBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasTableName(UsersTableName)
                .HasPrimaryKey(f => f.Id).AutoIncrement(AutoIncrementMethodType.Custom)
                .UseConverterForProperties<string>(NullAndTrimStringConverter.ConvertNull);

            modelBuilder.Entity<Permission>()
                .HasTableName(PermissionsTableName)
                .UseConverterForProperties<string>(NullAndTrimStringConverter.ConvertNull)
                .Property(f => f.Key).HasColumnName("PermissionKey");
        }
    }
}
