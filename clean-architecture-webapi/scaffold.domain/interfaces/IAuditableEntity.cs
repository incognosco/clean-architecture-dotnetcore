using System;

namespace Domain.Interfaces
{
    public interface IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastmodifiedBy { get; set; }
        public DateTime? LastmodifiedOn { get; set; }
    }
}