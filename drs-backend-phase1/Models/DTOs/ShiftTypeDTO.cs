//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/21 - 07:01:17
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class ShiftTypeDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public bool isDeleted { get; set; }

        public ShiftTypeDTO()
        {
        }

        public ShiftTypeDTO(int id, string name, DateTime? dateCreated, DateTime? dateModified, bool isDeleted)
        {
			this.id = id;
			this.name = name;
			this.dateCreated = dateCreated;
			this.dateModified = dateModified;
			this.isDeleted = isDeleted;
        }
    }
}