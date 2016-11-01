using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Projection
{
    public static class ObjectPropertyAccessor
    {
        public static void SetValue<T>(T model, string propertyName, object value)
        {
            try
            {
                ObjectAccessor.Create(model)[propertyName] = value;
            }
            catch (InvalidCastException ex)
            {
                throw new Exception($"Unable to set value {value} in property {propertyName} on entity {model.GetType().Name}", ex);
            }
        }
    }
}
