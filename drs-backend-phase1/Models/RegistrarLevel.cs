//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace drs_backend_phase1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegistrarLevel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RegistrarLevel()
        {
            this.ProfileProfessionals = new HashSet<ProfileProfessional>();
           OnCreated();
        }
    
    	partial void OnCreated();
    
        public int id { get; set; }
        public string name { get; set; }
        public System.DateTime dateCreated { get; set; }
        public System.DateTime dateModified { get; set; }
        public bool isDeleted { get; set; }
        public byte[] rowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileProfessional> ProfileProfessionals { get; set; }
    }
}
