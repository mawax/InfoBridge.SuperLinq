using InfoBridge.SuperLinq.Core.QueryBuilders;
using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class StringRestrictionBuilderTests
    {
        [Fact]
        public void TestAnd()
        {
            var srb = new StringRestrictionBuilder()
                .And("contact.contact_id", EOperator.Equals, 1)
                .And("contact.name", EOperator.Equals, "InfoBridge Software");

            var restrictions = srb.GetRestrictions();
            Assert.NotEmpty(restrictions);
            Assert.Equal(2, restrictions.Count);

            Assert.Equal("contact.contact_id", restrictions[0].Name);
            Assert.Equal(InterRestrictionOperator.And, restrictions[0].InterOperator);

            //these should both be 0, as we dont support parenthesis right now, with the string restriction builder
            Assert.Equal(0, restrictions[0].InterParenthesis);
            Assert.Equal(0, restrictions[1].InterParenthesis);
            Assert.Equal("[I:1]", restrictions[0].Values[0]);
            Assert.Equal("InfoBridge Software", restrictions[1].Values[0]);
        }

        [Fact]
        public void TestIn()
        {
            var srb = new StringRestrictionBuilder()
                .And("contact.contact_id", EOperator.OneOf, 1, 3, 7);

            var restrictions = srb.GetRestrictions();
            Assert.NotEmpty(restrictions);
            Assert.Equal("[I:1]", restrictions[0].Values[0]);
            Assert.Equal("[I:3]", restrictions[0].Values[1]);
            Assert.Equal("[I:7]", restrictions[0].Values[2]);
        }

        [Fact]
        public void TestOr()
        {
            var srb = new StringRestrictionBuilder()
                .And("contact.contact_id", EOperator.Equals, 1)
                .Or("contact.contact_id", EOperator.Equals, 2);

            var restrictions = srb.GetRestrictions();
            Assert.NotEmpty(restrictions);
            Assert.Equal(InterRestrictionOperator.Or, restrictions[0].InterOperator);
        }
    }
}
