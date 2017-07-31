using System;

namespace drs_backend_phase1.Models
{
    public partial class JobType
    {
        partial void OnCreated()
        {
            dateCreated=DateTime.Now;
            dateModified=DateTime.Now;
            isClinical = true;
            isGmcRequired = false;
            isHcpcRequired = false;
            isNmcRequired = false;
            isDeleted = false;
            id = 7;
            name = "Receptionist";
        }
    }

    public partial class ProfileProfessional
    {
        partial void OnCreated()
        {
            isDeleted = false;
            jobTypeId = 7;

        }
    }
}