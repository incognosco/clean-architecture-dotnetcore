using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scaffold.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Scaffold.Domain.Entities
{
    [Table("user")]
    public partial class User : IBaseEntity, IAuditableEntity, ISoftDelete    
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("firstname", TypeName = "character varying")]
        public string Firstname { get; set; }
        [Column("lastname", TypeName = "character varying")]
        public string Lastname { get; set; }
        [Column("email", TypeName = "character varying")]
        public string Email { get; set; }
        [Column("profile_id")]
        public int ProfileId { get; set; }
        [Column("client_key", TypeName = "character varying")]
        public string ClientKey { get; set; }
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
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}
