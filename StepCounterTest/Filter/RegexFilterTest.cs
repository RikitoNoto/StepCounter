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
        private Filter_if checkFiltering(string[] expect_strings, string src, string start, string end)
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

            return filter;
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
        public void 複数文字の開始文字列と終了文字列を渡したときにフィルタリングできること()
        {
            this.checkFiltering(new string[] { "aaaaa\n" }, "S↓↓↓\naaaaa\nE↑↑↑", "S↓↓↓\n", "E↑↑↑");
        }

        [TestMethod, TestCategory("Filtering")]
        public void 同じ開始文字列と終了文字列を渡したときにフィルタリングできること()
        {
            this.checkFiltering(new string[] { "aaaaa" }, "MaaaaaM", "M", "M");
        }

        [TestMethod, TestCategory("Filtering")]
        public void フィルタリング前に開始文字列の出現回数が0であること()
        {
            Filter_if filter = new RegexFilter_c();

            Assert.AreEqual(0, filter.StartStringCount);    // 開始文字列の出現回数が0であること
        }

        [TestMethod, TestCategory("Filtering")]
        public void フィルタリング後に開始文字列の出現回数を取得できること()
        {
            Filter_if filter = this.checkFiltering(new string[] { "aaaaa" }, "SaaaaaEE", "S", "E");

            Assert.AreEqual(1, filter.StartStringCount);    // 開始文字列の出現回数が1であること
        }

        [TestMethod, TestCategory("Filtering")]
        public void 複数の開始文字列の出現回数を取得できること()
        {
            Filter_if filter = this.checkFiltering(new string[] { "aSaaSSSaa" }, "SaSaaSSSaaE", "S", "E");

            Assert.AreEqual(5, filter.StartStringCount);    // 開始文字列の出現回数が5であること
        }

        [TestMethod, TestCategory("Filtering")]
        public void フィルタリング前に終了文字列の出現回数が0であること()
        {
            Filter_if filter = new RegexFilter_c();

            Assert.AreEqual(0, filter.EndStringCount);    // 終了文字列の出現回数が0であること
        }

        [TestMethod, TestCategory("Filtering")]
        public void フィルタリング後に終了文字列の出現回数を取得できること()
        {
            Filter_if filter = this.checkFiltering(new string[] { "aaaaa" }, "SaaaaaEE", "S", "E");

            Assert.AreEqual(2, filter.EndStringCount);    // 開始文字列の出現回数が2であること
        }

        [TestMethod, TestCategory("Filtering")]
        public void 複数の終了文字列の出現回数を取得できること()
        {
            Filter_if filter = this.checkFiltering(new string[] { "a" }, "SaEaEEEaEaE", "S", "E");

            Assert.AreEqual(6, filter.EndStringCount);    // 開始文字列の出現回数が5であること
        }
    }
}
