using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StepCounter;
using Counter;

namespace StepCounterTest
{
    [TestClass]
    public class LineCounterTest
    {
        [TestMethod, TestCategory("CountLines")]
        //[Description("Test Case Description")]
        public void 空の文字列を入力したときに0を返すこと()
        {
            var line_counter = new PrivateType(typeof(LineCounter_c));
        }
    }
}
