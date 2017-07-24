//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/23 - 07:58:30
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class AgencyDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class BankDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class BaseDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool? isDeleted { get; set; }
        public string name { get; set; }
        public int teamId { get; set; }

    }

    public partial class CCGDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class DocumentTypeDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class EmailDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public string from { get; set; }
        public int id { get; set; }
        public bool? isDeleted { get; set; }
        public string subject { get; set; }
        public string text { get; set; }
        public string to { get; set; }
    }

    public partial class IndemnityProviderDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class JobTypeDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool? isClinical { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isGmcRequired { get; set; }
        public bool? isHcpcRequired { get; set; }
        public bool? isNmcRequired { get; set; }
        public string name { get; set; }
    }

    public partial class PaymentCategoryDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class ProfileDocumentDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateExpires { get; set; }
        public DateTime? dateModified { get; set; }
        public DateTime? dateObtained { get; set; }
        public DocumentTypeDTO DocumentType { get; set; }
        public int? documentTypeId { get; set; }

        public int id { get; set; }
        public bool isDeleted { get; set; }
        public string mimeType { get; set; }
        public string multerFileName { get; set; }
        public string originalFileName { get; set; }
        public int? profileId { get; set; }
    }

    public partial class ProfileDTO
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string address5 { get; set; }
        public string adEmailAddress { get; set; }
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string firstName { get; set; }

        public string homeEmail { get; set; }
        public string homePhone { get; set; }
        public int id { get; set; }
        public bool isComplete { get; set; }
        public bool isDeleted { get; set; }
        public bool isInactive { get; set; }
        public string lastName { get; set; }
        public string middleNames { get; set; }
        public string mobilePhone { get; set; }
        public string nhsEmail { get; set; }
        public string postcode { get; set; }
        public List<ProfileDocumentDTO> ProfileDocuments { get; set; }
        public ProfileFinanceDTO ProfileFinance { get; set; }
        public int? profileFinanceId { get; set; }
        public ProfileProfessionalDTO ProfileProfessional { get; set; }
        public int? profileProfessionalId { get; set; }
        public byte? roleID { get; set; }
        public SecurityRoleDTO SecurityRole { get; set; }
        public bool smsEnabled { get; set; }
        public List<SpecialNoteDTO> SpecialNotes { get; set; }
    }
    public partial class ProfileFinanceDTO
    {
        public BankDTO Bank { get; set; }
        public string bankAccountNumber { get; set; }
        public int? bankId { get; set; }
        public string bankSortCode { get; set; }
        public string buildingSocietyRollNumber { get; set; }
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool isDeleted { get; set; }
        public bool isIc24Staff { get; set; }
        public string nationalInsuranceNumber { get; set; }
        public string payrollNumber { get; set; }
    }

    public partial class ProfilePaymentCategoryDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool isDefault { get; set; }
        public bool isDeleted { get; set; }
        public int? paymentCategoryId { get; set; }

        public int? profileProfessionalId { get; set; }
    }

    public partial class ProfileProfessionalDTO
    {
        public AgencyDTO Agency { get; set; }
        public int? agencyId { get; set; }
        public BaseDTO Base_ { get; set; }
        public int? baseId { get; set; }
        public CCGDTO CCG { get; set; }
        public int? ccgId { get; set; }
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public string gmcNumber { get; set; }
        public string hcpcNumber { get; set; }
        public int id { get; set; }
        public DateTime? indemnityExpiryDate { get; set; }
        public string indemnityNumber { get; set; }
        public IndemnityProviderDTO IndemnityProvider { get; set; }
        public int? indemnityProviderId { get; set; }
        public bool isDeleted { get; set; }
        public bool isPremium { get; set; }
        public bool isRegistrarGreen { get; set; }
        public bool isTrainer { get; set; }
        public JobTypeDTO JobType { get; set; }
        public int? jobTypeId { get; set; }
        public string nmcNumber { get; set; }
        public string performersList { get; set; }
        public bool performersListChecked { get; set; }
        public string performersListCheckedBy { get; set; }
        public DateTime? performersListCheckedDate { get; set; }
        public List<ProfilePaymentCategoryDTO> ProfilePaymentCategories { get; set; }
        public List<ProfileShiftTypeDTO> ProfileShiftTypes { get; set; }
        public RegisteredSurgeryDTO RegisteredSurgery { get; set; }
        public int? registeredSurgeryId { get; set; }
        public RegistrarLevelDTO RegistrarLevel { get; set; }
        public int? registrarLevelId { get; set; }
        public string registrarTrainer { get; set; }
        public DateTime? registrationExpiryDate { get; set; }
        public SubTypeDTO SubType { get; set; }
        public int? subTypeId { get; set; }
        public int? teamId { get; set; }
    }

    public partial class ProfileShiftTypeDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool isDeleted { get; set; }
        public bool? isUnderReview { get; set; }
        public int? profileProfessionalId { get; set; }
        public ShiftTypeDTO ShiftType { get; set; }
        public int? shiftTypeId { get; set; }
    }

    public partial class RegisteredSurgeryDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class RegistrarLevelDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class SecurityRoleDTO
    {
        public int? createdProfileID { get; set; }
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public bool? isDeleted { get; set; }
        public int? modifiedProfileID { get; set; }
        public byte roleId { get; set; }

        public string roleName { get; set; }

        public DateTime validFrom { get; set; }

        public DateTime validTo { get; set; }
    }

    public partial class ShiftTypeDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }

    public partial class SpecialNoteDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool? isDeleted { get; set; }
        public string noteText { get; set; }
        public int profileId { get; set; }
        public string userName { get; set; }
    }

    public partial class SubTypeDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }
        public bool isAgency { get; set; }
        public bool isDeleted { get; set; }
        public bool isRegistrar { get; set; }
        public string name { get; set; }
    }

    public partial class TeamDTO
    {
        //public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public int id { get; set; }

        public bool isDeleted { get; set; }
        public string name { get; set; }
    }
}