using InfoBridge.SuperLinq.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Tests.Unit.Helpers
{
    public class MockColumnConfigProvider : ColumnConfigProviderBase
    {
        public const string SUCCESS_COLUMN_NAME = "SuccessColumn";

        public override string GetColumnName(string[] configContext, string propertyName)
        {
            if (propertyName == "Test1" && configContext[0] == "Context1" && configContext[1] == "Context2")
            {
                return SUCCESS_COLUMN_NAME;
            }
            return null;
        }
    }
}
