using InfoBridge.SuperLinq.Core.Attributes;
using InfoBridge.SuperLinq.Core.Projection;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class DynamicPropertyHelperTests
    {
        [Fact]
        public void TestGetAllDbColumns()
        {
            IList<string> results = DynamicPropertyHelper.GetAllDbColumns(typeof(PropertyHelperTestCompany));

            Assert.NotEmpty(results);
            Assert.Equal(3, results.Count);
            Assert.Equal("id", results[0]);
            Assert.Equal("name", results[1]);
            Assert.Equal("activated", results[2]);
        }

        [Fact]
        public void TestGetAllDbColumnsPrefixed()
        {
            IList<string> results = DynamicPropertyHelper.GetAllDbColumnsPrefixed<PropertyHelperTestCompany>();

            Assert.NotEmpty(results);
            Assert.Equal(3, results.Count);
            Assert.Equal("testcompany.id", results[0]);
            Assert.Equal("testcompany.name", results[1]);
            Assert.Equal("testcompany.activated", results[2]);
        }

        [Theory]
        [InlineData("ActivatedRaw", true)]
        [InlineData("Activated", false)]
        public void TestGetColumnInfo(string propertyName, bool hasRawDbDataOn)
        {
            ColumnInfo result = DynamicPropertyHelper.GetColumnInfo(typeof(PropertyHelperTestCompany), propertyName);

            Assert.Equal("activated", result.Name);
            Assert.Equal(hasRawDbDataOn, result.UseRaw);
        }

        [Fact]
        public void TestGetColumnName()
        {
            string result = DynamicPropertyHelper.GetColumnName(typeof(PropertyHelperTestCompany), "Id");

            Assert.Equal("id", result);
        }

        [Fact]
        public void TestGetColumnNameMultiEntity()
        {
            string companyResult = DynamicPropertyHelper.GetColumnName(typeof(PropertyHelperTestCompany), "Name");
            string personResult = DynamicPropertyHelper.GetColumnName(typeof(TestPerson), "LastName");

            Assert.Equal("name", companyResult);
            Assert.Equal("lastname", personResult);
        }

        [Fact]
        public void TestGetFullDotSyntaxColumnName()
        {
            string result = DynamicPropertyHelper.GetFullDotSyntaxColumnName("person", "id");

            Assert.Equal("person.id", result);
        }

        [Fact]
        public void TestGetFullDotSyntaxColumnNameGeneric()
        {
            string result = DynamicPropertyHelper.GetFullDotSyntaxColumnName<PropertyHelperTestCompany>("id");
            Assert.Equal("testcompany.id", result);
        }

        [Theory]
        [InlineData("id", new[] { "PK", "Id" })]
        [InlineData("activated", new[] { "Activated", "ActivatedRaw" })]
        [InlineData("testcompany.name", new[] { "Name" })]
        public void TestGetPropertyNames(string columnName, string[] expected)
        {
            string[] results = DynamicPropertyHelper.GetPropertyNames(typeof(PropertyHelperTestCompany), columnName);

            Assert.Equal(expected, results);
        }

        [Fact]
        public void TestGetPropertyNamesGeneric()
        {
            string[] results = DynamicPropertyHelper.GetPropertyNames<PropertyHelperTestCompany>("id");

            Assert.Equal(new[] { "PK", "Id" }, results);
        }

        [Fact]
        public void TestGetTableName()
        {
            string name = DynamicPropertyHelper.GetTableName(new PropertyHelperTestCompany());

            Assert.Equal("testcompany", name);
        }
    }
}
