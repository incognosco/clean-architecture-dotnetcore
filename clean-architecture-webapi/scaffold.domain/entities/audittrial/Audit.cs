using System;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.AuditTrial 
{
    [Table("audit_trial")]
    public class Audit : IBaseEntity, IAuditableEntity, IMustHaveTenant
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public string? UserId { get; set; }
        [Column("type")]
        public string? Type { get; set; }
        [Column("table_name")]
        public string? TableName { get; set; }
        [Column("date_time")] 
        public DateTime DateTime { get; set; }
        [Column("old_values")]
        public string? OldValues { get; set; }
        [Column("new_values")]
        public string? NewValues { get; set; }
        [Column("affected_columns")]
        public string AffectedColumns { get; set; } = null!;
        [Column("primary_key")]
        public string PrimaryKey { get; set; } = null!;
        [Column("client_key")]
        public string TenantKey { get; set; } = null!;

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
