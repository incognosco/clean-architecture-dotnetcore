using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Scaffold.Domain.Entities
{
    [Table("role")]
    public partial class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; }
        [Column("description", TypeName = "character varying")]
        public string Description { get; set; }
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
        public bool? Isdeleted { get; set; }
        [Column("is_active", TypeName = "character varying")]
        public string Isactive { get; set; }
        [Column("isdefault")]
        public bool? Isdefault { get; set; }
    }
}
