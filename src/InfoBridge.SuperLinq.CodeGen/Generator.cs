using InfoBridge.SuperLinq.Core.ModelBase;
using Microsoft.CSharp;
using SuperOffice.Data;
using SuperOffice.Data.SQL;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperOffice.Data.Dictionary.SoTable;

namespace InfoBridge.SuperLinq.CodeGen
{
    public class Generator
    {
        public CodeCompileUnit Build()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace nsModels = new CodeNamespace("InfoBridge.SuperLinq.Core.Models");
            nsModels.Imports.Add(new CodeNamespaceImport("System"));
            nsModels.Imports.Add(new CodeNamespaceImport("InfoBridge.SuperLinq.Core.ModelBase"));
            nsModels.Imports.Add(new CodeNamespaceImport("InfoBridge.SuperLinq.Core.Attributes"));
            compileUnit.Namespaces.Add(nsModels);

            for (int tableNumber = 0; tableNumber < 500; tableNumber++)
            {
                TableInfo ti = TablesInfo.GetTableInfo(tableNumber);
                if (ti != null)
                {
                    if (IsExcluded(ti.DbName) || ti.Definition.Kind != TableKind.System)
                    {
                        continue;
                    }

                    CodeTypeDeclaration clss = new CodeTypeDeclaration(FormatName(ti.DbName));
                    clss.BaseTypes.Add(GetBaseType(ti));
                    var tableAttr = new CodeAttributeDeclaration { Name = nameof(Core.Attributes.TableInfo) };
                    tableAttr.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(ti.DbName)));
                    clss.CustomAttributes.Add(tableAttr);
                    nsModels.Types.Add(clss);

                    foreach (FieldInfo fi in ti.All)
                    {
                        //filter out cs extra fields, fetching Definition to look at IsEjournalExtraField throws exceptions on some tables
                        if (fi.Name.StartsWith("x_")) { continue; }

                        string name = fi.Name;
                        //we cannot have a class and propery with the same name
                        if (name.ToLower() == ti.DbName.ToLower()) { name = name + "_"; }
                        AddProperty(clss, name, GetDbName(fi), ConvertType(fi, fi.DataType));

                        if (GetPrimaryKeyField(ti) == fi.Name)
                        {
                            AddProperty(clss, "PK", GetDbName(fi), typeof(int));
                        }
                    }
                }
            }

            return compileUnit;
        }

        private Type GetBaseType(TableInfo ti)
        {
            if (!new[] { "registered", "updated", "registered_associate_id", "updated_associate_id" }.Except(ti.All.Select(x => x.Name)).Any())
            {
                return typeof(IStandardModel);
            }
            else
            {
                return typeof(ISoModel);
            }
        }

        private Type ConvertType(FieldInfo fi, FieldDataType dataType)
        {
            Type type = null;
            switch (dataType)
            {
                case FieldDataType.dbBlob:
                    type = typeof(byte[]);
                    break;
                case FieldDataType.dbDateLocal: break;
                case FieldDataType.dbDateTimeLocal: break;
                case FieldDataType.dbDateTimeUTC: break;
                case FieldDataType.dbExtendedDateTimeLocal:
                case FieldDataType.dbExtendedDateTimeUTC:
                    type = typeof(DateTime);
                    break;
                case FieldDataType.dbDouble:
                    type = typeof(double);
                    break;
                case FieldDataType.dbInt:
                case FieldDataType.dbIntId:
                    type = typeof(int);
                    break;
                case FieldDataType.dbNull: break;
                case FieldDataType.dbShort:
                case FieldDataType.dbShortId:
                    type = typeof(int); //web services supply us with int
                    break;
                case FieldDataType.dbString:
                case FieldDataType.dbStringBlob:
                    type = typeof(string);
                    break;
                case FieldDataType.dbTimeLocal: break;
                case FieldDataType.dbUInt:
                    type = typeof(int); //web services supply us with int
                    break;
                case FieldDataType.dbUShort:
                    type = typeof(int); //web services supply us with int
                    break;
                case FieldDataType.dbIntIdArr:
                    type = typeof(int[]);
                    break;
                case FieldDataType.Undefined: break;
            }

            if (type == null)
            {
                throw new NotSupportedException(fi.Name + ": " + dataType.ToString());
            }

            return type;
        }

        public string GenerateString(CodeCompileUnit compileUnit)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            using (StringWriter sw = new StringWriter())
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
                tw.Close();
                sw.Flush();

                string result = "#pragma warning disable 1591" + Environment.NewLine;//disable Missing XML comment warnings
                return result + sw.ToString();
            }
        }

        private string GetDbName(FieldInfo fi)
        {
            //many tables throw an exception when fetching the DbName field. It seems that Name is always equal to DbName for those that don't.
            return fi.Name;
        }

        private bool IsExcluded(string table)
        {
            return false;
        }

        private string GetPrimaryKeyField(TableInfo ti)
        {
            if (ti.DbName == "SubscriptionUnitHeadingLink")
            {
                return "subscriptionunitheadinglink_id";
            }
            else
            {
                return ti.PrimaryKeyField.Name;
            }
        }

        private void AddProperty(CodeTypeDeclaration clss, string name, string dbName, Type type)
        {
            string fieldName = "_" + name;
            clss.Members.Add(new CodeMemberField
            {
                Type = new CodeTypeReference(type),
                Name = fieldName,
                Attributes = MemberAttributes.Private
            });

            var property = new CodeMemberProperty
            {
                Type = new CodeTypeReference(type),
                Name = FormatName(name),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));

            var fieldAttr = new CodeAttributeDeclaration { Name = nameof(Core.Attributes.ColumnInfo) };
            fieldAttr.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(dbName)));
            property.CustomAttributes.Add(fieldAttr);

            clss.Members.Add(property);
        }

        private string FormatName(string name)
        {
            bool endsUnderscore = name.EndsWith("_");
            string newName = "";
            foreach (string part in name.Split('_'))
            {
                newName += part.FirstToUpper();
            }
            if (endsUnderscore) { newName += "_"; }

            return newName;
        }
    }
}
