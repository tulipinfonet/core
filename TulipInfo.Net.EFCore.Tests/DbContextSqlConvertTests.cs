using Microsoft.VisualStudio.TestTools.UnitTesting;
using TulipInfo.Net.EFCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace TulipInfo.Net.EFCore.Tests
{
    [TestClass]
    public class DbContextSqlConvertTests
    {
        DbContextSqlConvert _convert = null;

        public DbContextSqlConvertTests()
        {

        }

        [TestInitialize]
        public void InitDbContext()
        {
            _convert = new DbContextSqlConvert(new TestDbContext());
        }

        [TestMethod]
        public void Test_ConvertToDeleteSql_Guid()
        {
            Expression<Func<EntityOne, bool>> exp = e => e.Id == Guid.Empty;
            var contextSql = _convert.ConvertToDeleteSql<EntityOne>(exp);
            Assert.AreEqual("Delete from EntityOnes Where (Id={0})", contextSql.Sql);
            Assert.AreEqual(1, contextSql.ParamValues.Count());
            Assert.AreEqual(Guid.Empty, contextSql.ParamValues.FirstOrDefault());
            
        }

        [TestMethod]
        public void Test_ConvertToDeleteSql_Different_Column()
        {
            Expression<Func<EntityOne, bool>> exp = e => e.DisplayName == "admin";
            var contextSql = _convert.ConvertToDeleteSql<EntityOne>(exp);
            Assert.AreEqual("Delete from EntityOnes Where (FullName={0})", contextSql.Sql);
            Assert.AreEqual(1, contextSql.ParamValues.Count());
            Assert.AreEqual("admin", contextSql.ParamValues.FirstOrDefault());
        }

        [TestMethod]
        public void Test_ConvertToDeleteSql_And()
        {
            DateTime dtm = new DateTime(2020, 10, 1);
            Expression<Func<EntityOne, bool>> exp = e => e.Age > 10 && e.CreatedDateTime > dtm;
            var contextSql = _convert.ConvertToDeleteSql<EntityOne>(exp);
            Assert.AreEqual("Delete from EntityOnes Where ((Age>{0}) AND (CreatedDateTime>{1}))", contextSql.Sql);
            Assert.AreEqual(2, contextSql.ParamValues.Count());
            Assert.AreEqual(10, Convert.ToInt32(contextSql.ParamValues.ElementAt(0)));
            Assert.AreEqual(dtm, Convert.ToDateTime(contextSql.ParamValues.ElementAt(1)));
        }

        [TestMethod]
        public void Test_ConvertToDeleteSql_Or()
        {
            DateTime dtm = new DateTime(2020, 11, 11);
            Expression<Func<EntityOne, bool>> exp = e => e.Age > 10 || e.CreatedDateTime > dtm;
            var contextSql = _convert.ConvertToDeleteSql<EntityOne>(exp);
            Assert.AreEqual("Delete from EntityOnes Where ((Age>{0}) OR (CreatedDateTime>{1}))", contextSql.Sql);
            Assert.AreEqual(2, contextSql.ParamValues.Count());
            Assert.AreEqual(10, Convert.ToInt32(contextSql.ParamValues.ElementAt(0)));
            Assert.AreEqual(dtm, Convert.ToDateTime(contextSql.ParamValues.ElementAt(1)));
        }

        [TestMethod]
        public void Test_ConvertToDeleteSql_And_Or()
        {
            DateTime dtm = new DateTime(2020, 11, 11);
            Expression<Func<EntityOne, bool>> exp = e => (e.Age > 10 && e.IsActive == false) || (e.Age < 10 && e.IsActive == true);
            var contextSql = _convert.ConvertToDeleteSql<EntityOne>(exp);
            Assert.AreEqual("Delete from EntityOnes Where (((Age>{0}) AND (IsActive={1})) OR ((Age<{2}) AND (IsActive={3})))", contextSql.Sql);
            Assert.AreEqual(4, contextSql.ParamValues.Count());
            Assert.AreEqual(10, Convert.ToInt32(contextSql.ParamValues.ElementAt(0)));
            Assert.AreEqual(false, Convert.ToBoolean(contextSql.ParamValues.ElementAt(1)));
            Assert.AreEqual(10, Convert.ToInt32(contextSql.ParamValues.ElementAt(2)));
            Assert.AreEqual(true, Convert.ToBoolean(contextSql.ParamValues.ElementAt(3)));
        }

        [TestMethod]
        public void Test_ConvertToUpdateSql()
        {
            Guid id = Guid.NewGuid();
            var contextSql = _convert.ConvertToUpdateSql<EntityOne>(e => e.Id == id, () => new EntityOne
            {
                IsActive = false,
                DisplayName = "admin"
            });
            Assert.AreEqual("Update EntityOnes Set IsActive={1},FullName={2} Where (Id={0})", contextSql.Sql);
            Assert.AreEqual(3, contextSql.ParamValues.Count());
            Assert.AreEqual(id, Guid.Parse(contextSql.ParamValues.ElementAt(0).ToString()));
            Assert.AreEqual(false, Convert.ToBoolean(contextSql.ParamValues.ElementAt(1)));
            Assert.AreEqual("admin", Convert.ToString(contextSql.ParamValues.ElementAt(2)));
        }
    }
}
