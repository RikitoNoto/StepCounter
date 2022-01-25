//using System;

namespace Counter
{
    public class LineCounter_c : Counter_if
    {
        public int Count(string src)
        {
            // 改行文字の数
            int new_line_char_length = src.Replace("\n", "").Length;

            // 文字列の文字数 - 改行文字数 + 1
            return (src.Length - new_line_char_length) + 1;
        }
    }
}
