using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Scaffold.Domain.Interfaces;

#nullable disable

namespace Scaffold.Domain.Entities
{
    [Table("user_role_mapping")]
    public partial class UserRoleMapping : IBaseEntity, IAuditableEntity, IMustHaveTenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("tenant_key", TypeName = "character varying")]
        public string TenantKey { get; set; }
        [Column("created_by", TypeName = "character varying")]
        public string CreatedBy { get; set; }
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
        [Column("lastmodified_by", TypeName = "character varying")]
        public string LastmodifiedBy { get; set; }
        [Column("lastmodified_on")]
        public DateTime? LastmodifiedOn { get; set; }
        [Column("deleted_by", TypeName = "character varying")]
        public string DeletedBy { get; set; }
        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }
    }
}
