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
    
    public partial class SecurityRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SecurityRole()
        {
            this.Profiles = new HashSet<Profile>();
            this.SecurityPermissions = new HashSet<SecurityPermission>();
        }
    
        public byte roleId { get; set; }
        public string roleName { get; set; }
        public System.DateTime validFrom { get; set; }
        public System.DateTime validTo { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateModified { get; set; }
        public Nullable<int> createdProfileID { get; set; }
        public Nullable<int> modifiedProfileID { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public byte[] rowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SecurityPermission> SecurityPermissions { get; set; }
    }
}
