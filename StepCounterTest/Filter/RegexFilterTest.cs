using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StepCounter;
using Filter;

namespace RegexFilterTest
{
    [TestClass]
    public class RegexFilterTest
    {
        /// <summary>
        ///     フィルタリングメソッドのチェックを行う。
        ///         ・予期していたマッチ数があっているか。
        ///         ・予期していたマッチ文字列があっているか。
        /// </summary>
        /// <param name="expect_string">予期している文字列の配列</param>
        /// <param name="src">フィルタリングする文字列</param>
        /// <param name="start_string">開始文字列</param>
        /// <param name="end_string">終了文字列</param>
        private void checkFiltering(string[] expect_strings, string src, string start, string end)
        {
            // フィルタークラスを作成
            Filter_if filter = new RegexFilter_c();
            // フィルタリングを実行
            string[] filter_strings = filter.Filtering(src, start, end);

            Assert.AreEqual(expect_strings.Length, filter_strings.Length);      // 配列サイズが正しいこと

            int i = 0;
            foreach(string expect in expect_strings)
            {
                Assert.AreEqual(expect, filter_strings[i]);    // 文字列が一致していること
                i++;
            }
        }

        [TestMethod, TestCategory("Filtering")]
        public void 異なる開始と終了文字列渡したときに間の文字列を返すこと()
        {
            this.checkFiltering(new string[] { "aaaaa" }, "saaaaae", "s", "e");
        }

        [TestMethod, TestCategory("Filtering")]
        public void 異なる開始と終了文字列が2つずつ存在している文字列を渡したときに2回間の文字列を返すこと()
        {
            this.checkFiltering(new string[] { "aaaaa", "bbbbb" }, "SaaaaaESbbbbbE", "S", "E");
        }

        [TestMethod, TestCategory("Filtering")]
        public void 複数行をフィルタリングできること()
        {
            this.checkFiltering(new string[] { "\naaaaa\n" }, "S\naaaaa\nE", "S", "E");
        }

        [TestMethod, TestCategory("Filtering")]
        public void 開始文字列と終了文字列を渡したときにフィルタリングできること()
        {
            this.checkFiltering(new string[] { "aaaaa" }, "MaaaaaM", "M", "M");
        }
    }
}
