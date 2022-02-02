using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StepCounter;
using Counter;

namespace LineCounterTest
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

        [TestMethod, TestCategory("count")]
        public void 改行文字が2文字の時に正しい行数を返すこと()
        {
            var counter = new LineCounter_c();
            LineCounter_c.NEW_LINE_STRING = "\r\n";
            Assert.AreEqual(4, counter.Count("1行目\r\n2行目\r\n3行目\r\n4行目"));
        }
    }
}
