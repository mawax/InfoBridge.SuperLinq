using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TableInfo : Attribute
    {
        public string Name { get; private set; }
        public bool NonDynamic { get; private set; }

        public TableInfo()
        {
            NonDynamic = true;
        }

        public TableInfo(string name)
        {
            Name = name;
        }
    }
}
