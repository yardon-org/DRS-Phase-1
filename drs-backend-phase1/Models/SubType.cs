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
    
    public partial class SubType
    {
        public string name { get; set; }
        public bool isAgency { get; set; }
        public bool isRegistrar { get; set; }
        public int id { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateModified { get; set; }
        public bool isDeleted { get; set; }
    }
}
