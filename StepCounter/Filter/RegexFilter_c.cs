using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Filter
{
    public class RegexFilter_c : Filter_if
    {
        /// <summary>
        ///     引数で受け取った開始文字列と終了文字列で正規表現を使用してフィルタリングし、
        ///     間の文字列を配列として返す。
        ///     フィルタリングは最短マッチを行う。
        /// </summary>
        /// <param name="src">フィルタリングする文字列</param>
        /// <param name="start_string">開始文字列</param>
        /// <param name="end_string">終了文字列</param>
        /// <returns>マッチした文字列の配列</returns>
        public string[] Filtering(string src, string start_string, string end_string)
        {
            List<string> match_strings = new List<string>();
            GroupCollection group_collection;

            string regex_string = $"{start_string}(?<filtering_string>.*?){end_string}";
            Match match = Regex.Match(src, regex_string, RegexOptions.Singleline);

            while(true)
            {
                if (match.Success)
                {
                    group_collection = match.Groups;
                    match_strings.Add(group_collection["filtering_string"].Value);
                    match = match.NextMatch();
                }
                else
                {
                    break;
                }
            }

            return match_strings.ToArray();
        }
    }
}
