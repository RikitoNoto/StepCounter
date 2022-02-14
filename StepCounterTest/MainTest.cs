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
        public void �R�}���h���C���������O�����J����Ă��邱��()
        {
            string[] args = { "A", "B" };
            Main_c.Main(args);

            CollectionAssert.AreEqual(args, Main_c.Args);
        }

        [TestMethod, TestCategory("Argments")]
        public void �I�v�V�����Ȃ��Ŏ��s�����Ƃ��ɃI�v�V���������������Ŏ擾�ł��邱��()
        {
            Main_c.Main(new string[0]);
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            CollectionAssert.AreEqual(expect_dictionary, Main_c.OptionArgs);
        }

        [TestMethod, TestCategory("Argments")]
        public void ss�I�v�V�����Ŏ��s�����Ƃ��ɃL�[���o�^����Ă��邱��()
        {
            Main_c.Main(new string[]{"-ss", "value"});

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_START_STRING));
        }

        [TestMethod, TestCategory("Argments")]
        public void ss�I�v�V�����Ŏ��s�����Ƃ��ɓn�����������o�^����Ă��邱��()
        {
            Main_c.Main(new string[] { "-ss", "value" });
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            expect_dictionary.Add(Main_c.OPTION_ID_E.OPTION_ID_START_STRING, new string[1] { "value" });

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_START_STRING));
            CollectionAssert.AreEqual(expect_dictionary[Main_c.OPTION_ID_E.OPTION_ID_START_STRING], Main_c.OptionArgs[Main_c.OPTION_ID_E.OPTION_ID_START_STRING]);
        }

        [TestMethod, TestCategory("Argments")]
        public void es�I�v�V�����Ŏ��s�����Ƃ��ɃL�[���o�^����Ă��邱��()
        {
            Main_c.Main(new string[]{"-es", "value"});

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_END_STRING));
        }

        [TestMethod, TestCategory("Argments")]
        public void es�I�v�V�����Ŏ��s�����Ƃ��ɓn�����������o�^����Ă��邱��()
        {
            Main_c.Main(new string[] { "-es", "value" });
            Dictionary<Main_c.OPTION_ID_E, string[]> expect_dictionary = new Dictionary<Main_c.OPTION_ID_E, string[]>();
            expect_dictionary.Add(Main_c.OPTION_ID_E.OPTION_ID_END_STRING, new string[1] { "value" });

            Assert.IsTrue(Main_c.OptionArgs.ContainsKey(Main_c.OPTION_ID_E.OPTION_ID_END_STRING));
            CollectionAssert.AreEqual(expect_dictionary[Main_c.OPTION_ID_E.OPTION_ID_END_STRING], Main_c.OptionArgs[Main_c.OPTION_ID_E.OPTION_ID_END_STRING]);
        }



        //TODO: ���C�����\�b�h���s����A�I�v�V�����Ȃ��̃R�}���h���C���������O�����J����Ă��邱��
        //TODO: ���݂̃I�v�V�������O�����J����Ă��邱��
        //TODO: �I�v�V�����ŊJ�n������ƏI����������w��ł��邱��
        //TODO: �I�v�V�����Ńf�B���N�g���T�����[����ݒ�ł��邱��
        //TODO: �I�v�V�����Ńt�@�C���T�����[����ݒ�ł��邱��
        //TODO: �I�v�V�����Ńw���v�o�͂ł��邱��
        //TODO: ���ʂ��o�͂ł��邱��
        //TODO: v�I�v�V�����ŏڍׂɏo�͂ł��邱��
        //TODO: ce�I�v�V�����ŃG���[���m���邱��
    }
}
