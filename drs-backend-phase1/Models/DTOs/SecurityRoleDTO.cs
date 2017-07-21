//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/21 - 07:01:16
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class SecurityRoleDTO
    {
        public byte roleId { get; set; }

        public string roleName { get; set; }

        public DateTime validFrom { get; set; }

        public DateTime validTo { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public int? createdProfileID { get; set; }

        public int? modifiedProfileID { get; set; }

        public bool? isDeleted { get; set; }

        public SecurityRoleDTO()
        {
        }

        public SecurityRoleDTO(byte roleId, string roleName, DateTime validFrom, DateTime validTo, DateTime? dateCreated, DateTime? dateModified, int? createdProfileID, int? modifiedProfileID, bool? isDeleted)
        {
			this.roleId = roleId;
			this.roleName = roleName;
			this.validFrom = validFrom;
			this.validTo = validTo;
			this.dateCreated = dateCreated;
			this.dateModified = dateModified;
			this.createdProfileID = createdProfileID;
			this.modifiedProfileID = modifiedProfileID;
			this.isDeleted = isDeleted;
        }
    }
}