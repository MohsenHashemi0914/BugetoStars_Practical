using Application.Contexts;
using Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Persistence.Contexts
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Audition operations

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (IsEntityAudition(entityType))
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertDate");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateDate");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveDate");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemoved");
                }
            }

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            #region Audition operations

            IEnumerable<EntityEntry> sendedEntries = ChangeTracker.Entries()
                .Where(x => x.State.Equals(EntityState.Added) ||
                x.State.Equals(EntityState.Modified) ||
                x.State.Equals(EntityState.Deleted));

            foreach (var entry in sendedEntries)
            {
                IEntityType? entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());

                if (IsEntityAudition(entityType))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            {
                                entry.Property("InsertDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        case EntityState.Modified:
                            {
                                entry.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        case EntityState.Deleted:
                            {
                                entry.Property("IsRemoved").CurrentValue = true;
                                entry.Property("RemoveDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            #endregion

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            #region Audition operations

            IEnumerable<EntityEntry> sendedEntries = ChangeTracker.Entries()
                .Where(x => x.State.Equals(EntityState.Added) ||
                x.State.Equals(EntityState.Modified) ||
                x.State.Equals(EntityState.Deleted));

            foreach (var entry in sendedEntries)
            {
                IEntityType? entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());

                if (IsEntityAudition(entityType))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            {
                                entry.Property("InsertDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        case EntityState.Modified:
                            {
                                entry.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        case EntityState.Deleted:
                            {
                                entry.Property("IsRemoved").CurrentValue = true;
                                entry.Property("RemoveDate").CurrentValue = DateTime.UtcNow;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            #endregion

            return base.SaveChangesAsync(cancellationToken);
        }

        #region Utilities

        private static bool IsEntityAudition(IEntityType? entityType) =>
            entityType is not null &&
            entityType.ClrType
            .GetCustomAttributes(typeof(AuditableAttribute), true)
            .Length > 0;

        private static bool IsEntityAudition(IMutableEntityType? entityType) =>
            entityType is not null &&
            entityType.ClrType
            .GetCustomAttributes(typeof(AuditableAttribute), true)
            .Length > 0;

        #endregion
    }
}