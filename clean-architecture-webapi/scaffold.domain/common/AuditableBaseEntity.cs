using Scaffold.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scaffold.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity, IAuditableEntity
    {
        [Column("created_by")]
        public string CreatedBy { get; set; } = null!;
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
        [Column("lastmodified_by")]
        public string LastmodifiedBy { get; set; } = null!;
        [Column("lastmodified_on")]
        public DateTime? LastmodifiedOn { get; set; }

      
    }
}
