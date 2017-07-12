namespace drs_backend_phase1.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUser
    {
        /// <summary>
        /// Gets or sets the active directory unique identifier.
        /// </summary>
        /// <value>
        /// The active directory unique identifier.
        /// </value>
        public string ActiveDirectoryGuid { get; set; }
        /// <summary>
        /// Gets or sets the active directory sid.
        /// </summary>
        /// <value>
        /// The active directory sid.
        /// </value>
        public string ActiveDirectorySid { get; set; }
    }
}