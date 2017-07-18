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
    
    public partial class ProfileDocument
    {
        public Nullable<int> documentTypeId { get; set; }
        public string originalFileName { get; set; }
        public string multerFileName { get; set; }
        public string mimeType { get; set; }
        public Nullable<System.DateTime> dateObtained { get; set; }
        public Nullable<System.DateTime> dateExpires { get; set; }
        public Nullable<int> profileId { get; set; }
        public int id { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateModified { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual Profile Profile { get; set; }
        public virtual Lookup Lookup { get; set; }
        public virtual DocumentType DocumentType { get; set; }
    }
}
