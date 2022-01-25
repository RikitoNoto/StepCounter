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
        public void ��̕��������͂����Ƃ���1��Ԃ�����()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(1, counter.Count(""));
        }

        [TestMethod, TestCategory("count")]
        public void ��s�̕��������͂����Ƃ���2��Ԃ�����()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(2, counter.Count("\n"));
        }
    }
}
