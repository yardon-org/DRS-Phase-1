//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/21 - 07:02:08
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class EventLogTypeDTO
    {
        public string key { get; set; }

        public string friendlyName { get; set; }

        public string description { get; set; }

        public string cssClass { get; set; }

        public EventLogTypeDTO()
        {
        }

        public EventLogTypeDTO(string key, string friendlyName, string description, string cssClass)
        {
			this.key = key;
			this.friendlyName = friendlyName;
			this.description = description;
			this.cssClass = cssClass;
        }
    }
}