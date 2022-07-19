using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class ObjectComparerTest
    {
        public class SimpleClass
        {
            public int Value1 { get; set; }
            public string Value2 { get; set; }
            public decimal Value3 { get; set; }
            public float Value4 { get; set; }
            public DateTime Value5 { get; set; }
            public long[] Value6 { get; set; }
            public Dictionary<string,double> Value7 { get; set; }
        }

        public class ContainerClass
        {
            public SimpleClass Simple { get; set; }
            public List<SimpleClass> SimpleList { get; set; }
            public Dictionary<string,SimpleClass> SimpleDic { get; set; }
        }

        [TestMethod]
        public void TestSimpleClass()
        {
            DateTime now = DateTime.UtcNow;
            SimpleClass sc = new SimpleClass()
            {
                Value1 = 1,
                Value2 = "v2",
                Value3 = 3.2M,
                Value4 = 4.4f,
                Value5 = now
            };
            ObjectComparer<SimpleClass> comparer = new ObjectComparer<SimpleClass>();
            comparer.Add(sc);
            Assert.AreEqual(false, comparer.IsChanged);

            sc.Value1 = 2;
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            comparer.Add(sc);
            Assert.AreEqual(false, comparer.IsChanged);

            sc.Value2 = "V2";
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value3 = 3.3M;
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value4 = 4.5f;
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value5 = sc.Value5.AddTicks(1);
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value6 = new long[] { 1, 2, 3 };
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value6 = new long[] { 1, 2 };
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value7 = new Dictionary<string, double>();
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);

            sc.Value7.Add("key", 1);
            comparer.Add(sc);
            Assert.AreEqual(true, comparer.IsChanged);


            comparer.Add(sc);
            Assert.AreEqual(false, comparer.IsChanged);
            comparer.Reset();
            Assert.AreEqual(false, comparer.IsChanged);
        }

        [TestMethod]
        public void TestContainerClass()
        {
            ContainerClass cc = new ContainerClass();
            ObjectComparer<ContainerClass> comparer = new ObjectComparer<ContainerClass>();
            comparer.Add(cc);
            Assert.AreEqual(false, comparer.IsChanged);

            cc.Simple = new SimpleClass();
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.Simple.Value1 = 1;
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.Simple = new SimpleClass() { Value1 = 1 };
            comparer.Add(cc);
            Assert.AreEqual(false, comparer.IsChanged);

            cc.SimpleList = new List<SimpleClass>();
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleList.Add(new SimpleClass()); 
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleList[0].Value1 = 1;
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleDic = new Dictionary<string, SimpleClass>();
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleDic.Add("key", null);
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleDic["key"] = new SimpleClass();
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);

            cc.SimpleDic["key"].Value1 = 1;
            comparer.Add(cc);
            Assert.AreEqual(true, comparer.IsChanged);
        }
    }
}
