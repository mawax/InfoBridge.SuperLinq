using InfoBridge.SuperLinq.Core;
using InfoBridge.SuperLinq.Core.LinqProvider;
using InfoBridge.SuperLinq.Tests.Unit.Helpers;
using SuperOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfoBridge.SuperLinq.Tests.Unit
{
    public class QueryableTests
    {
        [Fact]
        public void TestBasicRestrictions()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.Id == 0).ToList();
            var restrictions = manager.RestrictionBuilder.GetRestrictions();

            Assert.Null(manager.OrderByBuilder);
            Assert.Equal(0, manager.NoItems);
            Assert.Equal(0, manager.Page);
            Assert.NotNull(restrictions);
            Assert.Equal(1, restrictions.Count);
            Assert.Equal("testperson.id", restrictions[0].Name);
            Assert.False(manager.DoQuerySingleCalled);
        }

        [Fact]
        public void TestWhere1()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.Id > 0 || x.LastName == "Jansen").ToList();
            var restrictions = manager.RestrictionBuilder.GetRestrictions();

            Assert.Equal(2, restrictions.Count);

            Assert.Equal("testperson.id", restrictions[0].Name);
            Assert.Equal("greater", restrictions[0].Operator);
            Assert.Equal("[I:0]", restrictions[0].Values[0]);
            Assert.Equal(1, restrictions[0].InterParenthesis);

            Assert.Equal(InterRestrictionOperator.Or, restrictions[0].InterOperator);

            Assert.Equal("testperson.lastname", restrictions[1].Name);
            Assert.Equal("equals", restrictions[1].Operator);
            Assert.Equal("Jansen", restrictions[1].Values[0]);
            Assert.Equal(-1, restrictions[1].InterParenthesis);
        }

        [Fact]
        public void TestWhere2()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.OptIn == 1 && (x.LastName == "test" || x.Email != null)).ToList();
            var restrictions = manager.RestrictionBuilder.GetRestrictions();

            Assert.Equal(3, restrictions.Count);

            Assert.Equal("testperson.opt_in", restrictions[0].Name);
            Assert.Equal(1, restrictions[0].InterParenthesis);
            Assert.Equal(InterRestrictionOperator.And, restrictions[0].InterOperator);

            Assert.Equal("testperson.lastname", restrictions[1].Name);
            Assert.Equal(1, restrictions[1].InterParenthesis);
            Assert.Equal(InterRestrictionOperator.Or, restrictions[1].InterOperator);

            Assert.Equal("testperson.email", restrictions[2].Name);
            Assert.Equal(-2, restrictions[2].InterParenthesis);
        }

        [Fact]
        public void TestParenthesis1()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.Id == 1 || x.Id == 2 || x.Id == 3 || x.Id == 4).ToList();
            var restrictions = manager.RestrictionBuilder.GetRestrictions();

            Assert.Equal(4, restrictions.Count);

            //this result might seem strange, but is actually correct. The custom linq provider always works in restrictions pairs or 2: a left part and a right part.
            //Every query is translated to this
            Assert.Equal(3, restrictions[0].InterParenthesis);
            Assert.Equal(-1, restrictions[1].InterParenthesis);
            Assert.Equal(-1, restrictions[2].InterParenthesis);
            Assert.Equal(-1, restrictions[3].InterParenthesis);
        }

        [Fact]
        public void TestParenthesis2()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => (x.Id >= 1 && x.Id <= 3) || (x.Id > 10 && x.Id < 50)).ToList();
            var restrictions = manager.RestrictionBuilder.GetRestrictions();

            Assert.Equal(4, restrictions.Count);

            Assert.Equal(2, restrictions[0].InterParenthesis);
            Assert.Equal(-1, restrictions[1].InterParenthesis);
            Assert.Equal(1, restrictions[2].InterParenthesis);
            Assert.Equal(-2, restrictions[3].InterParenthesis);
        }

        [Fact]
        public void TestFirstOrDefault()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.Id == 123).FirstOrDefault();

            Assert.True(manager.DoQuerySingleCalled);
        }

        [Fact]
        public void TestFirst()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));

            try
            {
                var results = q.Where(x => x.Id == 123456).First();
            }
            catch (InvalidOperationException ex)
            {
                //we expect an invalidoperationexception with message Sequence contains no elements
                if (ex.Message != "Sequence contains no elements") { throw ex; }
            }
        }

        [Fact]
        public void TestSingleOrDefault()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.Where(x => x.Id == 123).SingleOrDefault();

            Assert.False(manager.DoQuerySingleCalled);
            Assert.Equal(2, manager.NoItems);
        }

        [Fact]
        public void TestSingle()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));

            try
            {
                var results = q.Where(x => x.Id == 123456).Single();
            }
            catch (InvalidOperationException ex)
            {
                //we expect an invalidoperationexception with message Sequence contains no elements
                if (ex.Message != "Sequence contains no elements") { throw ex; }
            }
        }

        [Fact]
        public void TestOrderByAsc()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.OrderBy(x => x.LastName).ToList();
            var orderBy = manager.OrderByBuilder.Get();

            Assert.NotNull(orderBy);
            Assert.Equal(1, orderBy.Count);
            Assert.Equal("testperson.lastname", orderBy[0].Name);
            Assert.Equal(OrderBySortType.ASC, orderBy[0].Direction);
        }

        [Fact]
        public void TestOrderByAscDesc()
        {
            var manager = SetupManager<TestPerson>();
            var q = new Queryable<TestPerson>(CreateExecutor(manager));
            var results = q.OrderBy(x => x.LastName).OrderByDescending(x => x.Id).ToList();
            var orderBy = manager.OrderByBuilder.Get();

            Assert.NotNull(orderBy);
            Assert.Equal(2, orderBy.Count);
            Assert.Equal("testperson.lastname", orderBy[0].Name);
            Assert.Equal(OrderBySortType.ASC, orderBy[0].Direction);
            Assert.Equal("testperson.id", orderBy[1].Name);
            Assert.Equal(OrderBySortType.DESC, orderBy[1].Direction);
        }

        private MockArchiveManager<T> SetupManager<T>()
        {
            return new MockArchiveManager<T>();
        }

        private ITypedQueryExecutor<T> CreateExecutor<T>(MockArchiveManager<T> manager)
        {
            return new QueryExecutor<T>(manager);
        }
    }
}
