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
        private int called_count_EnumerateFiles = 0;     // EnumerateFiles ���\�b�h���Ă΂ꂽ��
        private List<List<string>> args_EnumerateFiles = new List<List<string>>();  // EnumerateFiles ���\�b�h���Ă΂ꂽ���̈���
        private List<string> return_value_EnumerateFiles = new List<string>();      // EnumerateFiles ���\�b�h�̕Ԃ�l

        private int called_count_EnumerateDirectories = 0;                                  // EnumerateDirectories ���\�b�h���Ă΂ꂽ��
        private List<string> args_EnumerateDirectories_path = new List<string>();                   // EnumerateDirectories ���\�b�h���Ă΂ꂽ���̈���path
        private List<SearchOption> args_EnumerateDirectories_search_option = new List<SearchOption>();    // EnumerateDirectories ���\�b�h���Ă΂ꂽ���̈���search_option
        private Dictionary<string, List<string>> return_value_EnumerateDirectories = new Dictionary<string, List<string>>();// EnumerateDirectories ���\�b�h�̕Ԃ�l


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

        /// <summary>EnumerateFiles���\�b�h��Mock�ƂƂ��ɌĂяo��</summary>
        /// <param name="path">�p�X</param>
        private void callExploreWithMock(string path)
        {
            this.callExploreWithMock(path, null);
        }

        /// <summary>EnumerateFiles���\�b�h��Mock�ƂƂ��ɌĂяo��</summary>
        /// <param name="path">�p�X</param>
        /// <param name="func">�R�[���o�b�N�֐�</param>
        private void callExploreWithMock(string path, Func<string, bool> func)
        {
            // Mock�Ń��\�b�h���s
            PoseContext.Isolate(delegate ()
            {
                FileExplorer_c.Explore(path, func); // Explore���\�b�h�Ăяo��

            }, this.EnumerateDirectoriesMock(path, func), this.EnumerateFilesMock(path, func));
        }

        /// <summary>EnumerateFiles��mock</summary>
        /// <param name="path">�p�X</param>
        /// <param name="func">�R�[���o�b�N�֐�</param>
        private Shim EnumerateFilesMock(string path, Func<string, bool> func)
        {
            Func<string, IEnumerable<string>> spy_func = delegate (string _path)
            {
                return this.EnumerateFilesSpy(_path); // �X�p�C�֐��̌Ăяo��
            };

            // Mock�̍쐬
            Shim enumerate_files_mock = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(spy_func);

            return enumerate_files_mock;
        }

        /// <summary>EnumerateFiles��mock</summary>
        /// <param name="path">�p�X</param>
        /// <param name="func">�R�[���o�b�N�֐�</param>
        private Shim EnumerateDirectoriesMock(string path, Func<string, bool> func)
        {
            Func<string, string, SearchOption, IEnumerable<string>> spy_func = delegate (string _path, string _search_pattern, SearchOption _option)
            {
                return EnumerateDirectoriesSpy(_path, _search_pattern, _option); // �X�p�C�֐��̌Ăяo��
            };

            // Mock�̍쐬
            Shim enumerate_directories_mock = Shim.Replace(() => Directory.EnumerateDirectories(Is.A<string>(), Is.A<string>(), Is.A<SearchOption>())).With(spy_func);

            return enumerate_directories_mock;
        }

        /// <summary>
        ///     Explore���\�b�h���Ăяo���R�[���o�b�N�֐����������Ăяo����邱�ƃ`�F�b�N����
        ///     �ϒ������Ŏ󂯎�����������A�R�[���o�b�N�֐����Ăяo����邱�Ƃ��`�F�b�N����B
        /// </summary>
        /// <param name="paths">
        ///     �R�[���o�b�N�����ɓn��������
        /// </param>
        private void checkCallBackFunc_Explore(params string[] paths)
        {

            List<string> call_back_args = new List<string>(); // �R�[���o�b�N�֐����Ăяo���ꂽ�Ƃ��̈���
            int call_count = 0; // �R�[���o�b�N�֐����Ăяo���ꂽ��

            // �R�[���o�b�N�֐�
            Func<string, bool> call_back_func = delegate (string path)
            {
                call_back_args.Add(path);   // ���������X�g�ɕۑ�
                call_count += 1;            // �Ăяo���񐔂����Z
                return false;
            };

            foreach (string path in paths)
            {
                this.return_value_EnumerateFiles.Add(path); // mock���ꂽEnumerateFiles�̕Ԃ�l���X�g��path��ǉ�
            }

            this.callExploreWithMock("src", call_back_func); // mock���ꂽEnumerateFiles���\�b�h���Ăяo��

            Assert.AreEqual(paths.Length, call_count);         // �R�[���o�b�N���\�b�h���������񐔌Ă΂�Ă��邱��

            for (int i = 0; i < paths.Length; i++)
            {
                Assert.AreEqual(paths[i], call_back_args[i]);     // �R�[���o�b�N�֐��Ăяo�����̈���������������
            }
        }

        /// <summary>EnumerateFiles���\�b�h�̃X�p�C���\�b�h</summary>
        /// <param name="path">�p�X</param>
        /// <returns>�p�X��T�����Č��������t�@�C����List</returns>
        private IEnumerable<string> EnumerateFilesSpy(string path)
        {
            this.called_count_EnumerateFiles++;     // EnumerateFiles���\�b�h���Ă΂ꂽ�񐔂��C���N�������g
            List<string> args = new List<string>(); // �󂯎���������p�̃��X�g���쐬
            args.Add(path); // ������ǉ�
            this.args_EnumerateFiles.Add(args); // �������X�g�������o�ɒǉ�

            return this.return_value_EnumerateFiles;
        }

        /// <summary>EnumerateDirectories���\�b�h�̃X�p�C���\�b�h</summary>
        /// <param name="path">�p�X</param>
        /// <returns>�p�X��T�����Č��������f�B���N�g����List</returns>
        private IEnumerable<string> EnumerateDirectoriesSpy(string path, string search_pattern, SearchOption option)
        {
            this.called_count_EnumerateDirectories++;     // EnumerateFiles���\�b�h���Ă΂ꂽ�񐔂��C���N�������g
            this.args_EnumerateDirectories_path.Add(path); // path��ǉ�
            this.args_EnumerateDirectories_search_option.Add(option);
            System.Console.WriteLine(this.return_value_EnumerateDirectories.ContainsKey(path));

            if (this.return_value_EnumerateDirectories.ContainsKey(path))
            {
                return this.return_value_EnumerateDirectories[path];
            }

            return new List<string>();
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void �����œn�����󕶎����EnumerateFiles���Ăяo������()
        {
            this.callExploreWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // �����͋�̕�����ł��邱��
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void �����œn�����p�X��EnumerateFiles���Ăяo������()
        {
            this.callExploreWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // �����͋�̕�����ł��邱��
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void EnumerateFiles���Ԃ���1�̃t�@�C���������ɃR�[���o�b�N�֐������s���邱��()
        {
            this.checkCallBackFunc_Explore("file.c");
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void EnumerateFiles���Ԃ���2�̃t�@�C���������ɃR�[���o�b�N�֐������s���邱��()
        {
            this.checkCallBackFunc_Explore("file1.c", "file2.c");
        }

        [TestMethod, TestCategory("Explore")]
        [Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        public void �����œn�����󕶎����EnumerateDirectories���Ăяo������()
        {
            this.callExploreWithMock("");

            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "");
        }

        [TestMethod, TestCategory("Explore")]
        [Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        public void �����œn�����������EnumerateDirectories���Ăяo������()
        {
            this.callExploreWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "src");
        }

        //NOTE: �ċA������EnumerateDirectories �̃I�v�V�����Őݒ�ł���B
        //      �I�v�V�������������n����邩���e�X�g����B

        [TestMethod, TestCategory("Explore")]
        [Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        public void EnumerateFiles���ċA�I�v�V�����t���ŌĂяo������()
        {
            this.callExploreWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles == 1);
            Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
            Assert.AreEqual(this.args_EnumerateDirectories_path[0], "src");
            Assert.AreEqual(this.args_EnumerateDirectories_search_option[0], SearchOption.AllDirectories);
        }

        //[TestMethod, TestCategory("Explore")]
        //[Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        //public void �q�f�B���N�g�������݂��Ȃ��Ƃ�EnumerateFiles��1��Ăяo������()
        //{
        //    this.callExploreWithMock("src");

        //    Assert.IsTrue(this.called_count_EnumerateFiles == 1);
        //    Assert.IsTrue(this.called_count_EnumerateDirectories > 0);
        //    Assert.AreEqual(this.args_EnumerateDirectories_path[0][0], "src");
        //}

        //[TestMethod, TestCategory("Explore")]
        //[Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        //public void �q�f�B���N�g����1���݂���Ƃ�EnumerateFiles��2��Ăяo������()
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
        //[Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        //public void �q�f�B���N�g����2���݂���Ƃ�EnumerateFiles��3��Ăяo������()
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
        //[Description("�f�B���N�g���̒T�����ċA�I�ɂł��邱��")]
        //public void ���f�B���N�g����1���݂���Ƃ�EnumerateFiles��3��Ăяo������()
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

        //TODO: Enumerate���\�b�h�̗�O �u�p�X����v��⑫
        //TODO: Enumerate���\�b�h�̗�O �u���݂��Ȃ��p�X�v��⑫
        //TODO: �t�@�C�������[��
        //TODO: �f�B���N�g�������[��
    }
}
