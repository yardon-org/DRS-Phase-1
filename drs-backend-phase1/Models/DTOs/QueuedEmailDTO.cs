//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.3.0.0 (entitiestodtos.codeplex.com).
//     Timestamp: 2017/07/21 - 07:00:51
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System.Text;
using System.Collections.Generic;
using System;

namespace drs_backend_phase1.Models.DTOs
{
    public partial class QueuedEmailDTO
    {
        public float retryCount { get; set; }

        public float status { get; set; }

        public int id { get; set; }

        public DateTime? dateCreated { get; set; }

        public DateTime? dateModified { get; set; }

        public bool? isDeleted { get; set; }

        public int email { get; set; }

        public QueuedEmailDTO()
        {
        }

        public QueuedEmailDTO(float retryCount, float status, int id, DateTime? dateCreated, DateTime? dateModified, bool? isDeleted, int email)
        {
			this.retryCount = retryCount;
			this.status = status;
			this.id = id;
			this.dateCreated = dateCreated;
			this.dateModified = dateModified;
			this.isDeleted = isDeleted;
			this.email = email;
        }
    }
}