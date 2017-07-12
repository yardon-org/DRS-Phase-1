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
    
    public partial class Profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profile()
        {
            this.SpecialNotes = new HashSet<SpecialNote>();
            this.ProfileDocuments = new HashSet<ProfileDocument>();
        }
    
        public string firstName { get; set; }
        public string middleNames { get; set; }
        public string lastName { get; set; }
        public System.DateTime dateOfBirth { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string address5 { get; set; }
        public string postcode { get; set; }
        public string homePhone { get; set; }
        public string mobilePhone { get; set; }
        public string homeEmail { get; set; }
        public string nhsEmail { get; set; }
        public Nullable<bool> smsEnabled { get; set; }
        public Nullable<bool> isInactive { get; set; }
        public Nullable<bool> isComplete { get; set; }
        public int id { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateModified { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public Nullable<int> profileProfessionalId { get; set; }
        public Nullable<int> profileFinanceId { get; set; }
        public string adEmailAddress { get; set; }
        public Nullable<byte> roleID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialNote> SpecialNotes { get; set; }
        public virtual ProfileProfessional ProfileProfessional { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileDocument> ProfileDocuments { get; set; }
        public virtual ProfileFinance ProfileFinance { get; set; }
    }
}
