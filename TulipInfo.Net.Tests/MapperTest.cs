using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace TulipInfo.Net.Tests
{
    [TestClass]
    public class MapperTest
    {
        public class SubClass
        {
            public string Name { get; set; }
            public decimal Cost { get; set; }
        }

        public enum ClassKind
        {
            Primary,
            Secondary,
            Third,
            Fourth,
            Fifth,
            Six
        }

        public class SourceClass
        {
            public  int IntValue { get; set; }
            public int[] IntArray { get; set; }
            public List<int> IntList { get; set; }
            public Dictionary<string,int> IntDics { get; set; }
            public SubClass Sub { get; set; }
            public List<SubClass> SubList { get; set; }
            public ClassKind PrimaryKind { get; set; }
            public ClassKind SecondaryKind { get; set; }
            public string ThirdKind { get; set; }
            public int FourthKind { get; set; }
            public ClassKind FifthKind { get; set; }
            public ClassKind SixKind { get; set; }
        }

        public class TargetClass
        {
            public int IntValue { get; set; }
            public int[] IntArray { get; set; }
            public List<int> IntList { get; set; }
            public Dictionary<string, int> IntDics { get; set; }
            public SubClass Sub { get; set; }
            public List<SubClass> SubList { get; set; }
            public ClassKind PrimaryKind { get; set; }
            public ClassKind SecondaryKind { get; set; }

            public ClassKind ThirdKind { get; set; }
            public ClassKind FourthKind { get; set; }
            public string FifthKind { get; set; }
            public int SixKind { get; set; }
        }

        public class GuidTestClass
        {
            public Guid GValue { get; set; }
            public Guid? NGValue { get; set; }
        }

        public class GuidTestClass2
        {
            public Guid? GValue { get; set; }
            public Guid NGValue { get; set; }
        }

        [TestMethod]
        public void TestMap()
        {
            SourceClass source = new SourceClass()
            {
                IntValue = 1,
                IntArray = new int[] { 1, 2, 3 },
                IntList = new List<int>() { 4, 5, 6 }
            };
            Dictionary<string, int> dics = new Dictionary<string, int>();
            dics.Add("a", 1);
            dics.Add("b", 2);
            source.IntDics = dics;
            source.Sub = new SubClass() { Name = "n1", Cost = 1.2M };
            source.SubList = new List<SubClass>()
            {
                new SubClass(){ Name="n2",Cost=2.2M },
                new SubClass(){ Name="n3",Cost=3.2M }
            };
            source.PrimaryKind = ClassKind.Primary;
            source.SecondaryKind = ClassKind.Secondary;
            source.ThirdKind = "Third";
            source.FourthKind = 3;
            source.FifthKind = ClassKind.Fifth;
            source.SixKind = ClassKind.Six;


            TargetClass target = Mapper.Map<TargetClass>(source);

            Assert.AreEqual(1, target.IntValue);

            Assert.AreEqual(3, target.IntArray.Length);
            Assert.AreEqual(3, target.IntArray[2]);

            Assert.AreEqual(3, target.IntList.Count);
            Assert.AreEqual(6, target.IntList[2]);

            Assert.AreEqual(2, target.IntDics.Count);
            Assert.AreEqual(1, target.IntDics["a"]);
            Assert.AreEqual(2, target.IntDics["b"]);

            Assert.IsNotNull(target.Sub);
            Assert.AreEqual("n1", target.Sub.Name);
            Assert.AreEqual(1.2M, target.Sub.Cost);

            Assert.IsNotNull(target.SubList);
            Assert.AreEqual(2, target.SubList.Count);
            Assert.AreEqual("n2", target.SubList[0].Name);
            Assert.AreEqual(2.2M, target.SubList[0].Cost);
            Assert.AreEqual("n3", target.SubList[1].Name);
            Assert.AreEqual(3.2M, target.SubList[1].Cost);

            Assert.AreEqual(ClassKind.Primary, target.PrimaryKind);
            Assert.AreEqual(ClassKind.Secondary, target.SecondaryKind);
            Assert.AreEqual(ClassKind.Third, target.ThirdKind);
            Assert.AreEqual(ClassKind.Fourth, target.FourthKind);
            Assert.AreEqual("Fifth", target.FifthKind);
            Assert.AreEqual(5, target.SixKind);
        }

        [TestMethod]
        public void TestMap_Guid()
        {
            GuidTestClass source = new GuidTestClass()
            {
                GValue = Guid.NewGuid()
            };


            GuidTestClass target = Mapper.Map<GuidTestClass>(source);

            Assert.AreEqual(source.GValue, target.GValue);
            Assert.IsNull(target.NGValue);

            source.NGValue = Guid.NewGuid();
            target = Mapper.Map<GuidTestClass>(source);

            Assert.AreEqual(source.GValue, target.GValue);
            Assert.AreEqual(source.NGValue, target.NGValue);

            source = new GuidTestClass()
            {
                GValue = Guid.NewGuid()
            };

            GuidTestClass2 target2 = Mapper.Map<GuidTestClass2>(source);
            Assert.AreEqual(source.GValue, target2.GValue);
            Assert.AreEqual(Guid.Empty, target2.NGValue);

            source.NGValue = Guid.NewGuid();
            target2 = Mapper.Map<GuidTestClass2>(source);
            Assert.AreEqual(source.GValue, target2.GValue);
            Assert.AreEqual(source.NGValue, target2.NGValue);

        }

    }
}
