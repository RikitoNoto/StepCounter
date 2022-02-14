using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;
using System;
using System.IO;
using System.IO.Enumeration;
using System.Collections.Generic;
using StepCounter;

namespace MainTest
{
    [TestClass]
    public class MainTest
    {

        [TestInitialize]
        public void SetUp()
        {
        }

        [TestMethod, TestCategory("Argments")]
        public void コマンドライン引数が外部公開されていること()
        {
            string[] args = { "A", "B" };
            Main_c.Main(args);

            CollectionAssert.AreEqual(args, Main_c.Args);
        }

        [TestMethod, TestCategory("Argments")]
        public void オプションなしで実行したときにオプション引数が無しで取得できること()
        {
            Main_c.Main(new string[0]);
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            CollectionAssert.AreEqual(expect_dictionary, Main_c.OptionArgs);
        }

        [TestMethod, TestCategory("Argments")]
        public void ssオプションで実行したときにキーが登録されていること()
        {
            Main_c.Main(new string[]{"-ss", "value"});

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_START_STRING));
        }

        [TestMethod, TestCategory("Argments")]
        public void ssオプションで実行したときに渡した引数が登録されていること()
        {
            Main_c.Main(new string[] { "-ss", "value" });
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            expect_dictionary.Add(Main_c.OPTION_ID_E.OPTION_ID_START_STRING, new string[1] { "value" });

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_START_STRING));
            CollectionAssert.AreEqual(expect_dictionary[Main_c.OPTION_ID_E.OPTION_ID_START_STRING], Main_c.OptionArgs[Main_c.OPTION_ID_E.OPTION_ID_START_STRING]);
        }

        [TestMethod, TestCategory("Argments")]
        public void esオプションで実行したときにキーが登録されていること()
        {
            Main_c.Main(new string[]{"-es", "value"});

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_END_STRING));
        }

        [TestMethod, TestCategory("Argments")]
        public void esオプションで実行したときに渡した引数が登録されていること()
        {
            Main_c.Main(new string[] { "-es", "value" });
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            expect_dictionary.Add(Main_c.OPTION_ID_E.OPTION_ID_END_STRING, new string[1] { "value" });

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_END_STRING));
            CollectionAssert.AreEqual(expect_dictionary[Main_c.OPTION_ID_E.OPTION_ID_END_STRING], Main_c.OptionArgs[Main_c.OPTION_ID_E.OPTION_ID_END_STRING]);
        }



        //TODO: メインメソッド実行時後、オプションなしのコマンドライン引数が外部公開されていること
        //TODO: 現在のオプションが外部公開されていること
        //TODO: オプションで開始文字列と終了文字列を指定できること
        //TODO: オプションでディレクトリ探索ルールを設定できること
        //TODO: オプションでファイル探索ルールを設定できること
        //TODO: オプションでヘルプ出力できること
        //TODO: 結果を出力できること
        //TODO: vオプションで詳細に出力できること
        //TODO: ceオプションでエラー検知すること
    }
}
