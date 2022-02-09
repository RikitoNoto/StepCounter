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

        [TestInitialize]
        public void SetUp()
        {
            this.called_count_EnumerateFiles = 0;
            this.args_EnumerateFiles.Clear();
            this.return_value_EnumerateFiles.Clear();
        }

        /// <summary>EnumerateFiles���\�b�h��Mock�ƂƂ��ɌĂяo��</summary>
        /// <param name="path">�p�X</param>
        private void callEnumerateFilesWithMock(string path)
        {
            this.callEnumerateFilesWithMock(path, null);
        }

        /// <summary>EnumerateFiles���\�b�h��Mock�ƂƂ��ɌĂяo��</summary>
        /// <param name="path">�p�X</param>
        /// <param name="func">�R�[���o�b�N�֐�</param>
        private void callEnumerateFilesWithMock(string path, Func<string, bool> func)
        {
            Func<string, IEnumerable<string>> spy_func = delegate(string _path)
            {
                return this.EnumerateFilesSpy(_path); // �X�p�C�֐��̌Ăяo��
            };

            // Mock�̍쐬
            Shim consoleShim = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(spy_func);

            // Mock�Ń��\�b�h���s
            PoseContext.Isolate(delegate()
            {
                FileExplorer_c.Explore(path, func); // Explore���\�b�h�Ăяo��
            }, consoleShim);
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

            foreach(string path in paths)
            {
                this.return_value_EnumerateFiles.Add(path); // mock���ꂽEnumerateFiles�̕Ԃ�l���X�g��path��ǉ�
            }

            this.callEnumerateFilesWithMock("src", call_back_func); // mock���ꂽEnumerateFiles���\�b�h���Ăяo��

            Assert.AreEqual(paths.Length, call_count);         // �R�[���o�b�N���\�b�h���������񐔌Ă΂�Ă��邱��

            for(int i=0; i < paths.Length; i++)
            {
                Assert.AreEqual(paths[i], call_back_args[i]);     // �R�[���o�b�N�֐��Ăяo�����̈���������������
            }
        }

        /// <summary>EnumerateFiles���\�b�h�̃X�p�C���\�b�h</summary>
        /// <param name="path">�p�X</param>
        /// <returns>�p�X��T�����Č��������t�@�C����List</returns>
        public IEnumerable<string> EnumerateFilesSpy(string path)
        {
            this.called_count_EnumerateFiles++;     // EnumerateFiles���\�b�h���Ă΂ꂽ�񐔂��C���N�������g
            List<string> args = new List<string>(); // �󂯎���������p�̃��X�g���쐬
            args.Add(path); // ������ǉ�
            this.args_EnumerateFiles.Add(args); // �������X�g�������o�ɒǉ�

            return this.return_value_EnumerateFiles;
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void �����œn�����󕶎����EnumerateFiles���Ăяo������()
        {
            this.callEnumerateFilesWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // �����͋�̕�����ł��邱��
        }

        [TestMethod, TestCategory("Explore")]
        [Description("��̃f�B���N�g���̒T�����ł��邱��")]
        public void �����œn�����p�X��EnumerateFiles���Ăяo������()
        {
            this.callEnumerateFilesWithMock("src");

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
    }
}
