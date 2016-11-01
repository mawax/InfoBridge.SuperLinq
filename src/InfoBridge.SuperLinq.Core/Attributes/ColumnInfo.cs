using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ColumnInfo : Attribute
    {
        public string Name { get; private set; }
        public bool UseRaw { get; set; }

        public ColumnInfo(string name)
        {
            Name = name;
        }
    }
}
