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
    
    public partial class SecurityGroup
    {
        public byte groupId { get; set; }
        public string groupName { get; set; }
        public System.DateTime validFrom { get; set; }
        public System.DateTime validTo { get; set; }
    }
}
