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
        ///     �t�B���^�����O���\�b�h�̃`�F�b�N���s���B
        ///         �E�\�����Ă����}�b�`���������Ă��邩�B
        ///         �E�\�����Ă����}�b�`�����񂪂����Ă��邩�B
        /// </summary>
        /// <param name="expect_string">�\�����Ă��镶����̔z��</param>
        /// <param name="src">�t�B���^�����O���镶����</param>
        /// <param name="start_string">�J�n������</param>
        /// <param name="end_string">�I��������</param>
        private void checkFiltering(string[] expect_strings, string src, string start, string end)
        {
            // �t�B���^�[�N���X���쐬
            Filter_if filter = new RegexFilter_c();
            // �t�B���^�����O�����s
            string[] filter_strings = filter.Filtering(src, start, end);

            Assert.AreEqual(expect_strings.Length, filter_strings.Length);      // �z��T�C�Y������������

            int i = 0;
            foreach(string expect in expect_strings)
            {
                Assert.AreEqual(expect, filter_strings[i]);    // �����񂪈�v���Ă��邱��
                i++;
            }
        }

        [TestMethod, TestCategory("Filtering")]
        public void �قȂ�J�n�ƏI��������n�����Ƃ��ɊԂ̕������Ԃ�����()
        {
            this.checkFiltering(new string[] { "aaaaa" }, "saaaaae", "s", "e");
        }

        [TestMethod, TestCategory("Filtering")]
        public void �قȂ�J�n�ƏI��������2�����݂��Ă��镶�����n�����Ƃ���2��Ԃ̕������Ԃ�����()
        {
            this.checkFiltering(new string[] { "aaaaa", "bbbbb" }, "SaaaaaESbbbbbE", "S", "E");
        }

        [TestMethod, TestCategory("Filtering")]
        public void �����s���t�B���^�����O�ł��邱��()
        {
            this.checkFiltering(new string[] { "\naaaaa\n" }, "S\naaaaa\nE", "S", "E");
        }

        [TestMethod, TestCategory("Filtering")]
        public void �J�n������ƏI���������n�����Ƃ��Ƀt�B���^�����O�ł��邱��()
        {
            this.checkFiltering(new string[] { "aaaaa" }, "MaaaaaM", "M", "M");
        }
    }
}
