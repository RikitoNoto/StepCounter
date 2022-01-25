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
        public void ‹ó‚Ì•¶š—ñ‚ğ“ü—Í‚µ‚½‚Æ‚«‚É1‚ğ•Ô‚·‚±‚Æ()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(1, counter.Count(""));
        }

        [TestMethod, TestCategory("count")]
        public void ˆês‚Ì•¶š—ñ‚ğ“ü—Í‚µ‚½‚Æ‚«‚É2‚ğ•Ô‚·‚±‚Æ()
        {
            var counter = new LineCounter_c();
            Assert.AreEqual(2, counter.Count("\n"));
        }
    }
}
