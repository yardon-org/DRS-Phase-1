using System.ComponentModel.DataAnnotations;

namespace drs_backend_phase1.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Audience
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the base64 secret.
        /// </summary>
        /// <value>
        /// The base64 secret.
        /// </value>
        [MaxLength(80)]
        [Required]
        public string Base64Secret { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}