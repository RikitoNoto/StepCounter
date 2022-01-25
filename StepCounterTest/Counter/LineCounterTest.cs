using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StepCounter;
using Counter;

namespace StepCounterTest
{
    [TestClass]
    public class LineCounterTest
    {
        [TestMethod, TestCategory("count")]
        //[Description("Test Case Description")]
        public void 空の文字列を入力したときに1を返すこと()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(1, counter.Count(""));
        }

        [TestMethod, TestCategory("count")]
        public void 一行の文字列を入力したときに2を返すこと()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(2, counter.Count("\n"));
        }
    }
}
