using Scaffold.Domain.Common;
using Scaffold.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Scaffold.Domain.Entities.Multitenancy
{
    [Table("tenant")]
    public partial class Tenant : IBaseEntity, IMustHaveTenant, IAuditableEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("tenant_key")]
        public string TenantKey { get; set; } = null!;
        [Column("tenant_code")]
        public string? TenantCode { get; set; }
        [Column("tenant_config", TypeName = "jsonb")]
        public string? TenantConfig { get; set; }
        [Column("uniqueid")]
        public string? UniqueId { get; set; }

        [NotMapped]
        public string? TenantId { get; set; }
        [Column("host_url")]
        [Scaffold.Domain.Common.Audit]
        public string? HostUrl { get; set; }
        [Column("admin_host_url")]
        [Scaffold.Domain.Common.Audit]
        public string? AdminHostUrl { get; set; }
        [Column("api_url")]
        [Scaffold.Domain.Common.Audit]
        public string? ApiUrl { get; set; }
        [Column("api_version")]
        public int? ApiVersion { get; set; }
        [Column("dbprovider")]
        public string? Dbprovider { get; set; }
        [Column("connection_string")]
        public string? ConnectionString { get; set; }
        [Column("schema")]
        public string? Schema { get; set; }
        [Column("created_by")]
        public string CreatedBy { get; set; } = null!;
        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }
        [Column("lastmodified_by")]
        public string LastmodifiedBy { get; set; } = null!;
        [Column("lastmodified_on")]
        public DateTime? LastmodifiedOn { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("valid_upto")]
        public DateTime? ValidUpto { get; set; }
        [Column("admin_email")]
        public string? AdminEmail { get; set; }
        [Column("assets_path")]
        [Scaffold.Domain.Common.Audit]
        public string? AssetsPath { get; set; }
        [Column("time_zone")]
        public string? TimeZone { get; set; }
        [Column("geo_id")]
        public string? GeoId { get; set; }
        [Column("geo_name")]
        public string? GeoName { get; set; }
    }
}
