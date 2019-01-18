using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ColumnInfo : Attribute
    {
        public string Name { get; private set; }
        public string PropertyAppliedName { get; private set; }
        public Type ConfigProviderClass { get; private set; }
        public string[] ConfigProviderParameters { get; private set; }
        public bool UseRaw { get; set; }

        public ColumnInfo(
            string name = null,
            [CallerMemberName] string propertyName = null,
            Type configProviderClass = null,
            string[] configProviderParameters = null)
        {
            Name = name;
            PropertyAppliedName = propertyName;

            ConfigProviderClass = configProviderClass;
            ConfigProviderParameters = configProviderParameters;
            SetNameFromConfigProvider();

            if (string.IsNullOrEmpty(Name))
            {
                Debug.Fail(nameof(Name) + " cannot be null or empty, specify it or use a ConfigProviderClass.");
            }
        }

        private void SetNameFromConfigProvider()
        {
            if (ConfigProviderClass == null)
            {
                return;
            }
            if (!ConfigProviderClass.IsClass || ConfigProviderClass.IsAbstract)
            {
                Debug.Fail("ConfigClass type is no class or is abstract.");
                return;
            }
            if (!ConfigProviderClass.IsSubclassOf(typeof(ColumnConfigProviderBase)))
            {
                Debug.Fail($"ConfigClass type is no subclass of {nameof(ColumnConfigProviderBase)}.");
                return;
            }

            var configProvider = (ColumnConfigProviderBase)Activator.CreateInstance(ConfigProviderClass);
            Name = configProvider.GetColumnName(ConfigProviderParameters, PropertyAppliedName);
        }
    }
}
