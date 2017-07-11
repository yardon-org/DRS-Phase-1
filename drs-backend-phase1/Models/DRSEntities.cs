namespace drs_backend_phase1.Models
{
    /// <summary>
    /// Partial Class for DRSEntities
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public partial class DRSEntities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DRSEntities"/> class.
        /// </summary>
        public DRSEntities()
            : base("name=DRSEntities")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}