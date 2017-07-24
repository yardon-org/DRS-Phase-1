using System;
using System.Data.Entity;

namespace drs_backend_phase1.Models
{
    /// <summary>
    /// DRSEntities override methods
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public partial class DRSEntities
    {
        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>
        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.Now;
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (entry.Property("dateModified").CurrentValue == null)
                        entry.Property("dateModified").CurrentValue = saveTime;


                    if (entry.Property("dateCreated").CurrentValue == null)
                        entry.Property("dateCreated").CurrentValue = saveTime;
                }
            }
            return base.SaveChanges();

        }
    }
}