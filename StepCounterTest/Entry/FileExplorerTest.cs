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
            this.callEnumerateFilesWithMock(path, null);
        }

        /// <summary>EnumerateFilesメソッドをMockとともに呼び出す</summary>
        /// <param name="path">パス</param>
        /// <param name="func">コールバック関数</param>
        private void callEnumerateFilesWithMock(string path, Func<string, bool> func)
        {
            Func<string, IEnumerable<string>> spy_func = delegate(string _path)
            {
                return this.EnumerateFilesSpy(_path); // スパイ関数の呼び出し
            };

            // Mockの作成
            Shim consoleShim = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(spy_func);

            // Mockでメソッド実行
            PoseContext.Isolate(delegate()
            {
                FileExplorer_c.Explore(path, func); // Exploreメソッド呼び出し
            }, consoleShim);
        }

        /// <summary>
        ///     Exploreメソッドを呼び出しコールバック関数が正しく呼び出されることチェックする
        ///     可変長引数で受け取った数だけ、コールバック関数が呼び出されることをチェックする。
        /// </summary>
        /// <param name="paths">
        ///     コールバック引数に渡される引数
        /// </param>
        private void checkCallBackFunc_Explore(params string[] paths)
        {

            List<string> call_back_args = new List<string>(); // コールバック関数が呼び出されたときの引数
            int call_count = 0; // コールバック関数が呼び出された回数

            // コールバック関数
            Func<string, bool> call_back_func = delegate (string path)
            {
                call_back_args.Add(path);   // 引数をリストに保存
                call_count += 1;            // 呼び出し回数を加算
                return false;
            };

            foreach(string path in paths)
            {
                this.return_value_EnumerateFiles.Add(path); // mockされたEnumerateFilesの返り値リストにpathを追加
            }

            this.callEnumerateFilesWithMock("src", call_back_func); // mockされたEnumerateFilesメソッドを呼び出し

            Assert.AreEqual(paths.Length, call_count);         // コールバックメソッドが正しい回数呼ばれていること

            for(int i=0; i < paths.Length; i++)
            {
                Assert.AreEqual(paths[i], call_back_args[i]);     // コールバック関数呼び出し時の引数が正しいこと
            }
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
        [Description("一つのディレクトリの探索ができること")]
        public void 引数で渡した空文字列でEnumerateFilesを呼び出すこと()
        {
            this.callEnumerateFilesWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // 一度でもEnumerateFilesメソッドが呼ばれていること
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // 引数は空の文字列であること
        }

        [TestMethod, TestCategory("Explore")]
        [Description("一つのディレクトリの探索ができること")]
        public void 引数で渡したパスでEnumerateFilesを呼び出すこと()
        {
            this.callEnumerateFilesWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // 一度でもEnumerateFilesメソッドが呼ばれていること
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // 引数は空の文字列であること
        }

        [TestMethod, TestCategory("Explore")]
        [Description("一つのディレクトリの探索ができること")]
        public void EnumerateFilesが返した1つのファイルを引数にコールバック関数を実行すること()
        {
            this.checkCallBackFunc_Explore("file.c");
        }

        [TestMethod, TestCategory("Explore")]
        [Description("一つのディレクトリの探索ができること")]
        public void EnumerateFilesが返した2つのファイルを引数にコールバック関数を実行すること()
        {
            this.checkCallBackFunc_Explore("file1.c", "file2.c");
        }
    }
}
