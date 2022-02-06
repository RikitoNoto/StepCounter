using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pose;
using System;
using System.IO;
using System.IO.Enumeration;
using System.Collections.Generic;
using StepCounter;
using Entry;

namespace EntryTest
{
    [TestClass]
    public class FileExplorerTest
    {
        private int called_count_EnumerateFiles = 0;     // EnumerateFiles メソッドが呼ばれた回数
        private List<List<string>> args_EnumerateFiles = new List<List<string>>();  // EnumerateFiles メソッドが呼ばれた時の引数
        private List<string> return_value_EnumerateFiles = new List<string>();      // EnumerateFiles メソッドの返り値

        [TestInitialize]
        public void SetUp()
        {
            this.called_count_EnumerateFiles = 0;
            this.args_EnumerateFiles.Clear();
            this.return_value_EnumerateFiles.Clear();
        }

        /// <summary>EnumerateFilesメソッドをMockとともに呼び出す</summary>
        /// <param name="path">パス</param>
        private void callEnumerateFilesWithMock(string path)
        {
            // Mockの作成
            Shim consoleShim = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(
                delegate (string _path)
                {
                    return this.EnumerateFilesSpy(_path); // スパイ関数の呼び出し
                });

            // MOCKでメソッド実行
            PoseContext.Isolate(() =>
            {
                FileExplorer_c.Explore(path); // Exploreメソッド呼び出し
            }, consoleShim);
        }

        /// <summary>EnumerateFilesメソッドのスパイメソッド</summary>
        /// <param name="path">パス</param>
        /// <returns>パスを探索して見つかったファイルのList</returns>
        public IEnumerable<string> EnumerateFilesSpy(string path)
        {
            this.called_count_EnumerateFiles++;     // EnumerateFilesメソッドが呼ばれた回数をインクリメント
            List<string> args = new List<string>(); // 受け取った引数用のリストを作成
            args.Add(path); // 引数を追加
            this.args_EnumerateFiles.Add(args); // 引数リストをメンバに追加

            return this.return_value_EnumerateFiles;
        }

        [TestMethod, TestCategory("Explore")]
        public void 引数で受け取った空文字列でEnumerateFilesを呼び出すこと()
        {
            this.callEnumerateFilesWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // 一度でもEnumerateFilesメソッドが呼ばれていること
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // 引数は空の文字列であること
        }

        [TestMethod, TestCategory("Explore")]
        public void 引数で受け取ったパスでEnumerateFilesを呼び出すこと()
        {
            this.callEnumerateFilesWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // 一度でもEnumerateFilesメソッドが呼ばれていること
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // 引数は空の文字列であること
        }

        [TestMethod, TestCategory("Explore")]
        public void EnumerateFilesが返したファイルを引数にコールバック関数を実行すること()
        {
            //this.callEnumerateFilesWithMock("src");

            //Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // 一度でもEnumerateFilesメソッドが呼ばれていること
            //Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // 引数は空の文字列であること
        }
    }
}
