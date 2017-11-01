using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    [TableInfo("testperson")]
    public class TestPerson : ISoModel
    {
        [ColumnInfo("id")]
        public virtual int Id { get; set; }

        [ColumnInfo("id")]
        public int PK { get; set; }

        [ColumnInfo("firstname")]
        public string FirstName { get; set; }

        [ColumnInfo("lastname")]
        public string LastName { get; set; }

        [ColumnInfo("email")]
        public string Email { get; set; }

        [ColumnInfo("opt_in")]
        public int OptIn { get; set; }

        [ColumnInfo("nullableInt")]
        public int? NullableInt { get; set; }
    }
}

