//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/25 - 01:58:24
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------

namespace drs_backend_phase1.Models.DTOs.SuperSlim
{
    public partial class SlimJobTypeDTO
    {
        public string name { get; set; }
        public byte[] rowVersion { get; set; }
    }


    public partial class SlimProfileDTO
    {
        public string firstName { get; set; }
        public string homeEmail { get; set; }
        public string homePhone { get; set; }
        public int id { get; set; }
        public bool isComplete { get; set; }
        public string lastName { get; set; }
        public string middleNames { get; set; }
        public string mobilePhone { get; set; }
        public string nhsEmail { get; set; }
        public SlimProfileProfessionalDTO ProfileProfessional { get; set; }
        public byte[] rowVersion { get; set; }
    }


    public partial class SlimProfileProfessionalDTO
    {
        public SlimJobTypeDTO JobType { get; set; }
        public byte[] rowVersion { get; set; }
    }

}