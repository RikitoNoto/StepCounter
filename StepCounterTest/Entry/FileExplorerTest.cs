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
            // Mock�̍쐬
            Shim consoleShim = Shim.Replace(() => Directory.EnumerateFiles(Is.A<string>())).With(
                delegate (string _path)
                {
                    return this.EnumerateFilesSpy(_path); // �X�p�C�֐��̌Ăяo��
                });

            // MOCK�Ń��\�b�h���s
            PoseContext.Isolate(() =>
            {
                FileExplorer_c.Explore(path); // Explore���\�b�h�Ăяo��
            }, consoleShim);
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
        public void �����Ŏ󂯎�����󕶎����EnumerateFiles���Ăяo������()
        {
            this.callEnumerateFilesWithMock("");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);    // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "");    // �����͋�̕�����ł��邱��
        }

        [TestMethod, TestCategory("Explore")]
        public void �����Ŏ󂯎�����p�X��EnumerateFiles���Ăяo������()
        {
            this.callEnumerateFilesWithMock("src");

            Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // �����͋�̕�����ł��邱��
        }

        [TestMethod, TestCategory("Explore")]
        public void EnumerateFiles���Ԃ����t�@�C���������ɃR�[���o�b�N�֐������s���邱��()
        {
            //this.callEnumerateFilesWithMock("src");

            //Assert.IsTrue(this.called_count_EnumerateFiles > 0);       // ��x�ł�EnumerateFiles���\�b�h���Ă΂�Ă��邱��
            //Assert.AreEqual(this.args_EnumerateFiles[0][0], "src");    // �����͋�̕�����ł��邱��
        }
    }
}
