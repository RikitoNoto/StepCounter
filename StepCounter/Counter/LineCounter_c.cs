//using System;

namespace Counter
{
    public class LineCounter_c : Counter_if
    {
        /// <summary>改行とみなす文字列</summary>
        static public string NEW_LINE_STRING = "\n";

        /// <summary>文字列の行数を返す。</summary>
        /// <param name="src">行数を数える文字列</param>
        /// <returns>行数</returns>
        public int Count(string src)
        {
            // 改行文字を抜いた文字数
            int string_length_except_new_line_char = src.Replace(LineCounter_c.NEW_LINE_STRING, "").Length;

            // (文字列の文字数 - 改行文字を抜いた文字数) / 改行文字列の文字数
            int new_line_length = (src.Length - string_length_except_new_line_char) / LineCounter_c.NEW_LINE_STRING.Length;

            // 行数 + 1
            return new_line_length + 1;
        }
    }
}
