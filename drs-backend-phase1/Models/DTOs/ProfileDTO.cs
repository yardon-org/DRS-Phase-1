//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/21 - 07:00:32
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class ProfileDTO
    {
        public string firstName { get; set; }

        public string middleNames { get; set; }

        public string lastName { get; set; }

        public DateTime dateOfBirth { get; set; }

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

        public bool smsEnabled { get; set; }

        public bool isInactive { get; set; }

        public bool isComplete { get; set; }

        public int id { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public bool isDeleted { get; set; }

        public int? profileProfessionalId { get; set; }

        public int? profileFinanceId { get; set; }

        public string adEmailAddress { get; set; }

        public byte? roleID { get; set; }

        public List<SpecialNoteDTO> SpecialNotes { get; set; }

        public ProfileProfessionalDTO ProfileProfessional { get; set; }

        public List<ProfileDocumentDTO> ProfileDocuments { get; set; }

        public ProfileFinanceDTO ProfileFinance { get; set; }

        public SecurityRoleDTO SecurityRole { get; set; }

      
    }
}