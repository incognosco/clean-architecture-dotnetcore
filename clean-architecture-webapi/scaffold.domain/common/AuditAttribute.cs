using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Scaffold.Domain.Common
{
    [AttributeUsage(AttributeTargets.Property )]
    public class AuditAttribute : Attribute
    {
        public AuditAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AuditTableAttribute : Attribute
    {
        public string TableName { get; }
        public AuditTableAttribute(string name)
        {
            TableName = name;
        }
    }
}
