using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.Exceptions;
using InfoBridge.SuperLinq.Core.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfoBridge.SuperLinq.Core.Projection
{
    public class DynamicPropertyHelper
    {
        /// <summary>
        /// Dictionary of ISuperOfficeModel type name, database column names and the properties linked to it
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, string[]>> EntityPropertyMap = new Dictionary<string, Dictionary<string, string[]>>();
        private static readonly Dictionary<string, Dictionary<string, ColumnInfo>> EntityColumnMap = new Dictionary<string, Dictionary<string, ColumnInfo>>();
        private static readonly Dictionary<string, string> EntityTableMap = new Dictionary<string, string>();

        private static object _lock = new object();

        public static IList<string> GetAllDbColumns(Type type)
        {
            EnsureMap(type);
            return GetPropertyMapForEntity(type).Keys.ToList();
        }

        public static IList<string> GetAllDbColumnsPrefixed<T>()
        {
            IList<string> ret = new List<string>();

            foreach (string s in GetAllDbColumns(typeof(T)))
            {
                ret.Add(DynamicPropertyHelper.GetFullDotSyntaxColumnName<T>(s));
            }
            return ret;
        }

        public static string[] GetPropertyNames<T>(string column)
        {
            return GetPropertyNames(typeof(T), column);
        }

        public static string[] GetPropertyNames(Type type, string column)
        {
            string trimmedColumn = column;

            string tableName = GetTableName(type);
            if (column.StartsWith(tableName + "."))
            {
                trimmedColumn = column.Remove(0, tableName.Length + 1);
            }
            return GetPropertyMapForEntity(type)[trimmedColumn];
        }

        public static string GetTableName(IModel model)
        {
            if (model == null) { throw new ArgumentNullException("model", "Unable to get table name: model is null"); }
            return GetTableName(model.GetType());
        }

        public static string GetColumnName<T>(string propertyName)
        {
            return GetColumnName(typeof(T), propertyName);
        }

        public static string GetColumnName(Type type, string propertyName)
        {
            return GetColumnInfo(type, propertyName).Name;
        }

        public static ColumnInfo GetColumnInfo(Type type, string propertyName)
        {
            EnsureMap(type);
            Dictionary<string, ColumnInfo> columnMap = GetColumnMapForEntity(type);
            if (!columnMap.ContainsKey(propertyName))
            {
                throw new SuperLinqException($"Unable to find column info for property '{propertyName}' in type '{type.FullName}'.");
            }
            return GetColumnMapForEntity(type)[propertyName];
        }

        public static string GetFullDotSyntaxColumnName<T>(string columnName)
        {
            return GetFullDotSyntaxColumnName(GetTableName(typeof(T)), columnName);
        }

        public static string GetFullDotSyntaxColumnName(string tableName, string columnName)
        {
            //make dotsyntax
            if (string.IsNullOrEmpty(tableName))
            {
                return columnName;
            }
            else
            {
                return string.Format("{0}.{1}", tableName, columnName);
            }
        }

        private static string GetTableName(Type type)
        {
            EnsureMap(type);

            string identifier = GetEntityIdentifier(type);
            if (!EntityTableMap.ContainsKey(identifier))
            {
                throw new Exception("Model type not found: " + identifier);
            }
            return EntityTableMap[identifier];
        }

        private static void EnsureMap(Type type)
        {
            string entityIdentifier = GetEntityIdentifier(type);
            //check if this type is initialized
            if (!EntityTableMap.ContainsKey(entityIdentifier))
            {
                lock (_lock)
                {
                    if (!EntityTableMap.ContainsKey(entityIdentifier))
                    {
                        TableInfo tableInfo = type.GetCustomAttribute<TableInfo>();
                        if (tableInfo == null)
                        {
                            throw new Exception("TableInfo attribute not found on type " + entityIdentifier);
                        }
                        else if (!tableInfo.NonDynamic && string.IsNullOrEmpty(tableInfo.Name))
                        {
                            throw new Exception("TableInfo attribute name is null or empty on type " + entityIdentifier);
                        }
                        else
                        {
                            var propertyMap = new Dictionary<string, string[]>();
                            var columnMap = new Dictionary<string, ColumnInfo>();

                            //loop all properties of the type we want to map
                            foreach (PropertyInfo p in type.GetProperties())
                            {
                                AddProperty(propertyMap, columnMap, p);
                            }

                            //add the new maps
                            EntityColumnMap.Add(entityIdentifier, columnMap);
                            EntityPropertyMap.Add(entityIdentifier, propertyMap);

                            //Last thing to do is adding the item to the entityTableMap. 
                            //Needs to be the last thing, as we use the item in this dictionary to check if this type is initialized. 
                            EntityTableMap.Add(entityIdentifier, tableInfo.Name);
                        }
                    }
                }
            }
        }

        private static void AddProperty(Dictionary<string, string[]> propertyMap, Dictionary<string, ColumnInfo> columnMap, PropertyInfo propertyInfo)
        {
            //extract our attribute and check if it exists
            ColumnInfo attr = (ColumnInfo)propertyInfo.GetCustomAttribute(typeof(ColumnInfo));
            if (attr != null)
            {
                if (!columnMap.ContainsKey(propertyInfo.Name))
                {
                    columnMap.Add(propertyInfo.Name, attr);
                }

                if (!propertyMap.ContainsKey(attr.Name))
                {
                    propertyMap.Add(attr.Name, new[] { propertyInfo.Name });
                }
                else
                {
                    var list = new List<string>(propertyMap[attr.Name]);
                    list.Add(propertyInfo.Name);
                    propertyMap[attr.Name] = list.ToArray();
                }
            }
        }

        private static string GetEntityIdentifier(Type type)
        {
            return type.FullName;
        }

        private static Dictionary<string, string[]> GetPropertyMapForEntity(Type type)
        {
            return EntityPropertyMap[GetEntityIdentifier(type)];
        }

        private static Dictionary<string, ColumnInfo> GetColumnMapForEntity(Type type)
        {
            return EntityColumnMap[GetEntityIdentifier(type)];
        }
    }
}
