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
    
    public partial class EventLog
    {
        public int id { get; set; }
        public string typeKey { get; set; }
        public Nullable<int> configId { get; set; }
        public Nullable<int> userId { get; set; }
        public string userName { get; set; }
        public System.DateTime createDate { get; set; }
        public string serverName { get; set; }
        public string properties { get; set; }
    
        public virtual EventLogConfig EventLogConfig { get; set; }
        public virtual EventLogType EventLogType { get; set; }
    }
}
