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

        [TestMethod, TestCategory("count")]
        public void ���s������2�����̎��ɐ������s����Ԃ�����()
        {
            var counter = new LineCounter_c();
            LineCounter_c.NEW_LINE_STRING = "\r\n";
            Assert.AreEqual(4, counter.Count("1�s��\r\n2�s��\r\n3�s��\r\n4�s��"));
        }
    }
}
