using InfoBridge.SuperLinq.Core.Archives;
using InfoBridge.SuperLinq.Core.Projection;
using InfoBridge.SuperLinq.Core.QueryBuilders;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using Moq;
using SuperOffice.Services75;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class ArchiveManagerTests
    {
        [Fact]
        public void TestDoQuery()
        {
            var executor = new Mock<IArchiveExecutor>();
            executor.
                Setup(x => x.GetItems(It.IsAny<ArchiveQueryParameters>(), It.IsAny<int>(), It.IsAny<int>())).
                Returns(new List<ArchiveListItem> { new ArchiveListItem {
                     ColumnData = new ColumnDataDictionary {
                         { "id", new ArchiveColumnData { DisplayValue = "[I:5]" } },
                         { "name", new ArchiveColumnData { DisplayValue = "infobridge" } },
                         { "activated", new ArchiveColumnData { DisplayValue = "[DT:01/31/2016 10:13:37.0000000]" } }
                     }
                } });

            var dateTimeConverter = new Mock<IDateTimeConverter>();
            dateTimeConverter.Setup(x => x.ConvertFromTimeZone(It.IsAny<DateTime>())).Returns(
                (DateTime ret) =>
                {
                    return ret.AddHours(6);
                });

            var man = new ArchiveManager<PropertyHelperTestCompany>(executor.Object,
                dateTimeConverter.Object,
                new ArchiveExecutionContextProvider(action => action.DateTimeToUTC = true));

            var result = man.DoQuery(new LinqRestrictionBuilder<PropertyHelperTestCompany>()).FirstOrDefault();
            Assert.Equal(5, result.Id);
            Assert.Equal("infobridge", result.Name);
            Assert.Equal(new DateTime(2016, 1, 31, 16, 13, 37).ToString(), result.Activated.ToLocalTime().ToString()); //this is utc, so we need to convert it to local time first
            Assert.Equal(new DateTime(2016, 1, 31, 10, 13, 37).ToString(), result.ActivatedRaw.ToString());
        }

        [Fact]
        public void TestDoQuerySkip()
        {
            var executor = new MockArchiveExecutor();
            var dateTimeConverter = new Mock<IDateTimeConverter>();
            var man = new ArchiveManager<PropertyHelperTestCompany>(executor, dateTimeConverter.Object, new ArchiveExecutionContextProvider());

            var result = man.DoQuery(new LinqRestrictionBuilder<PropertyHelperTestCompany>(), noItems: 0, skip: 13);

            Assert.Equal(1, executor.Page);
            Assert.Equal(0, executor.MaxItems);
            Assert.Equal(13, executor.Parameters.PageSize);
        }


        [Fact]
        public void TestDoQueryMax()
        {
            var executor = new MockArchiveExecutor();
            var man = new ArchiveManager<PropertyHelperTestCompany>(executor, new Mock<IDateTimeConverter>().Object, new ArchiveExecutionContextProvider());

            var result = man.DoQuery(new LinqRestrictionBuilder<PropertyHelperTestCompany>(), noItems: 15);

            Assert.Equal(0, executor.Page);
            Assert.Equal(15, executor.MaxItems);
            Assert.Equal(ArchiveQueryParameters.DEFAULT_PAGE_SIZE, executor.Parameters.PageSize);
        }

        [Fact]
        public void TestDoQueryTakeMax()
        {
            var executor = new MockArchiveExecutor();
            var man = new ArchiveManager<PropertyHelperTestCompany>(executor, new Mock<IDateTimeConverter>().Object, new ArchiveExecutionContextProvider());

            var result = man.DoQuery(new LinqRestrictionBuilder<PropertyHelperTestCompany>(), noItems: 15, skip: 30);

            Assert.Equal(3, executor.Page);
            Assert.Equal(15, executor.MaxItems);
            Assert.Equal(10, executor.Parameters.PageSize);
        }
    }
}
