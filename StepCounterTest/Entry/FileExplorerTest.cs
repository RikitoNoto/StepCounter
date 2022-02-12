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

        private int called_count_EnumerateDirectories = 0;                                  // EnumerateDirectories メソッドが呼ばれた回数
        private List<string> args_EnumerateDirectories_path = new List<string>();                   // EnumerateDirectories メソッドが呼ばれた時の引数path
        private List<SearchOption> args_EnumerateDirectories_search_option = new List<SearchOption>();    // EnumerateDirectories メソッドが呼ばれた時の引数search_option
        private Dictionary<string, List<string>> return_value_EnumerateDirectories = new Dictionary<string, List<string>>();// EnumerateDirectories メソッドの返り値


        [TestInitialize]
        public void SetUp()
        {
            this.called_count_EnumerateFiles = 0;
            this.args_EnumerateFiles.Clear();
            this.return_value_EnumerateFiles.Clear();

            this.called_count_EnumerateDirectories = 0;
            this.args_EnumerateDirectories_path.Clear();
            this.args_EnumerateDirectories_search_option.Clear();
            this.return_value_EnumerateDirectories.Clear();
        }

        /// <summary>EnumerateFilesメソッドをMockとともに呼び出す</summary>
        /// <param name="path">パス</param>
        private void callExploreWithMock(string path)
        {
            this.callExploreWithMock(path, null);
        }

        /// <summary>EnumerateFilesメソッドをMockとともに呼び出す</summary>
        /// <param name="path">パス</param>
        /// <param name="func">コールバック関数</param>
        private void callExploreWithMock(string path, Func<string, bool> func)
        {
            // Mockでメソッド実行
            PoseContext.Isolate(delegate ()
            {
                FileExplorer_c.Explore(path, func); // Exploreメソッド呼び出し

            }, this.EnumerateDirectoriesMock(path, func), this.EnumerateFilesMock(path, func));
        }

        /// <summary>EnumerateFilesのmock</summary>
        /// <param name="path">パス</param>
        /// <param name="func">コールバック関数</param>
        private Shim EnumerateFilesMock(string path, Func<string, bool> func)
        {
            Func<string, IEnumerable<string>> spy_func = delegate (string _path)
            {
                return this.EnumerateFilesSpy(_path); // スパイ関数の呼び出し
            };

            // Mockの作成
            Shim enumerate_files_mock = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(spy_func);

            return enumerate_files_mock;
        }

        /// <summary>EnumerateFilesのmock</summary>
        /// <param name="path">パス</param>
        /// <param name="func">コールバック関数</param>
        private Shim EnumerateDirectoriesMock(string path, Func<string, bool> func)
        {
            Func<string, string, SearchOption, IEnumerable<string>> spy_func = delegate (string _path, string _search_pattern, SearchOption _option)
            {
                return EnumerateDirectoriesSpy(_path, _search_pattern, _option); // スパイ関数の呼び出し
            };

            // Mockの作成
            Shim enumerate_directories_mock = Shim.Replace(() => Directory.EnumerateDirectories(Is.A<string>(), Is.A<string>(), Is.A<SearchOption>())).With(spy_func);

            return enumerate_directories_mock;
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

            foreach (string path in paths)
            {
                this.return_value_EnumerateFiles.Add(path); // mockされたEnumerateFilesの返り値リストにpathを追加
            }

            this.callExploreWithMock("src", call_back_func); // mockされたEnumerateFilesメソッドを呼び出し

            Assert.AreEqual(paths.Length, call_count);         // コールバックメソッドが正しい回数呼ばれていること

            for (int i = 0; i < paths.Length; i++)
            {
                Assert.AreEqual(paths[i], call_back_args[i]);     // コールバック関数呼び出し時の引数が正しいこと
            }
        }

        /// <summary>EnumerateFilesメソッドのスパイメソッド</summary>
        /// <param name="path">パス</param>
        /// <returns>パスを探索して見つかったファイルのList</returns>
        private IEnumerable<string> EnumerateFilesSpy(string path)
        {
            this.called_count_EnumerateFiles++;     // EnumerateFilesメソッドが呼ばれた回数をインクリメント
            List<string> args = new List<string>(); // 受け取った引数用のリストを作成
            args.Add(path); // 引数を追加
            this.args_EnumerateFiles.Add(args); // 引数リストをメンバに追加

            return this.return_value_EnumerateFiles;
        }

        /// <summary>EnumerateDirectoriesメソッドのスパイメソッド</summary>
        /// <param name="path">パス</param>
        /// <returns>パスを探索して見つかったディレクトリのList</returns>
        private IEnumerable<string> EnumerateDirectoriesSpy(string path, string search_pattern, SearchOption option)
        {
            this.called_count_EnumerateDirectories++;     // EnumerateFilesメソッドが呼ばれた回数をインクリメント
            this.args_EnumerateDirectories_path.Add(path); // pathを追加
            this.args_EnumerateDirectories_search_option.Add(option);
            System.Console.WriteLine(this.return_value_EnumerateDirectories.ContainsKey(path));

            if (this.return_value_EnumerateDirectories.ContainsKey(path))
            {
                return this.return_value_EnumerateDirectories[path];
            }

            return new List<string>();
        }

        [TestMethod, TestCategory("Explore")]
        [Description("一つのディレクトリの探索ができること")]
        public void 引数で渡した空文字列でEnumerateFilesを呼び出すこと()
        {
            this.callExploreWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // 一度でもEnumerateFilesメソッドが呼ばれていること
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // 引数は空の文字列であること
        }

        [TestMethod, TestCategory("Explore")]
        [Description("一つのディレクトリの探索ができること")]
        public void 引数で渡したパスでEnumerateFilesを呼び出すこと()
        {
            this.callExploreWithMock("src");

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

        [TestMethod, TestCategory("Explore")]
        [Description("ディレクトリの探索が再帰的にできること")]
        public void 引数で渡した空文字列でEnumerateDirectoriesを呼び出すこと()
        {
            this.callExploreWithMock("");

            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "");
        }

        [TestMethod, TestCategory("Explore")]
        [Description("ディレクトリの探索が再帰的にできること")]
        public void 引数で渡した文字列でEnumerateDirectoriesを呼び出すこと()
        {
            this.callExploreWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "src");
        }

        //NOTE: 再帰検索はEnumerateDirectories のオプションで設定できる。
        //      オプションが正しく渡されるかをテストする。

        [TestMethod, TestCategory("Explore")]
        [Description("ディレクトリの探索が再帰的にできること")]
        public void EnumerateFilesを再帰オプション付きで呼び出すこと()
        {
            this.callExploreWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles == 1);
            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "src");
            Assert.AreEqual(this.args_EnumerateDirectories_search_option[0], SearchOption.AllDirectories);
        }

        //[TestMethod, TestCategory("Explore")]
        //[Description("ディレクトリの探索が再帰的にできること")]
        //public void 子ディレクトリが存在しないときEnumerateFilesを1回呼び出すこと()
        //{
        //    this.callExploreWithMock("src");

        //    Assert.IsTrue(this.called_count_EnumerateFiles == 1);
        //    Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
        //    Assert.AreEqual(this.args_EnumerateDirectories_path[0][0], "src");
        //}

        //[TestMethod, TestCategory("Explore")]
        //[Description("ディレクトリの探索が再帰的にできること")]
        //public void 子ディレクトリが1つ存在するときEnumerateFilesを2回呼び出すこと()
        //{
        //    List<string> children = new List<string>();
        //    children.Add("child_dir");
        //    this.return_value_EnumerateDirectories["src"] = children;

        //    this.callExploreWithMock("src");

        //    Assert.IsTrue(this.called_count_EnumerateFiles == 2);
        //    Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");
        //    Assert.AreEqual(this.args_EnumerateFiles[1][0], "child_dir");
        //    Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
        //    Assert.AreEqual(this.args_EnumerateDirectories_path[0][0], "src");

        //}

        //[TestMethod, TestCategory("Explore")]
        //[Description("ディレクトリの探索が再帰的にできること")]
        //public void 子ディレクトリが2つ存在するときEnumerateFilesを3回呼び出すこと()
        //{
        //    List<string> children = new List<string>();
        //    children.Add("child_dir1");
        //    children.Add("child_dir2");
        //    this.return_value_EnumerateDirectories["src"] = children;

        //    this.callExploreWithMock("src");

        //    Assert.IsTrue(this.called_count_EnumerateFiles == 3);
        //    Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");
        //    Assert.AreEqual(this.args_EnumerateFiles[1][0], "child_dir1");
        //    Assert.AreEqual(this.args_EnumerateFiles[2][0], "child_dir2");
        //    Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
        //    Assert.AreEqual(this.args_EnumerateDirectories_path[0][0], "src");
        //}

        //[TestMethod, TestCategory("Explore")]
        //[Description("ディレクトリの探索が再帰的にできること")]
        //public void 孫ディレクトリが1つ存在するときEnumerateFilesを3回呼び出すこと()
        //{
        //    List<string> children = new List<string>();
        //    children.Add("child_dir1");
        //    this.return_value_EnumerateDirectories["src"] = children;

        //    List<string> grand_children = new List<string>();
        //    grand_children.Add("grand_child_dir1");
        //    this.return_value_EnumerateDirectories["child_dir1"] = grand_children;

        //    this.callExploreWithMock("src");

        //    Assert.IsTrue(this.called_count_EnumerateFiles == 3);
        //    Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");
        //    Assert.AreEqual(this.args_EnumerateFiles[1][0], "child_dir1");
        //    Assert.AreEqual(this.args_EnumerateFiles[2][0], "grand_child_dir1");
        //    Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
        //    Assert.AreEqual(this.args_EnumerateDirectories_path[0][0], "src");
        //}

        //TODO: Enumerateメソッドの例外 「パスが空」を補足
        //TODO: Enumerateメソッドの例外 「存在しないパス」を補足
        //TODO: ファイル名ルール
        //TODO: ディレクトリ名ルール
    }
}
