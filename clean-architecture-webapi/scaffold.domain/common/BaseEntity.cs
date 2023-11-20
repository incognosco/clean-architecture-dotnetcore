using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Scaffold.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        protected BaseEntity()
        {
            //Id = new Random().Next(999999);
        }

        protected List<string> serializableProperties { get; set; } = null!;

        protected void SetSerializableProperties(string fields)
        {
            if (!string.IsNullOrEmpty(fields))
            {
                var returnFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                serializableProperties = returnFields.ToList();
                return;
            }
            var members = this.GetType().GetMembers();

            serializableProperties = new List<string>();
            serializableProperties.AddRange(members.Select(x => x.Name).ToList());
        }

    }
}
