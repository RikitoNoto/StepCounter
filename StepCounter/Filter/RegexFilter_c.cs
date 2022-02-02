using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Filter
{
    public class RegexFilter_c : Filter_if
    {
        private int start_string_count = 0;
        private int end_string_count = 0;

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

            //FIXME: このメソッドを実行するとsrcを3回読み込む必要がある。
            this.start_string_count = this.countAppearances(src, start_string); // 開始文字列の回数をカウント
            this.end_string_count = this.countAppearances(src, end_string); // 終了文字列の回数をカウント

            return match_strings.ToArray();
        }

        /// <summary>
        ///     文字列の出現回数をカウントする。
        /// </summary>
        /// <param name="src">ソース文字列</param>
        /// <param name="search_string">検索文字列</param>
        /// <returns>検索文字列の出現回数</returns>
        private int countAppearances(string src, string search_string)
        {
            int count = 0;
            string target = src.Clone().ToString(); //  計算用にクローン

            // 文字列が見つからなくなるまでループ
            while (true)
            {
                int appearance_index = this.searchIndex(target, search_string); // 文字列のインデックスを取得

                // 文字列が見つからなかった場合
                if(appearance_index == -1)
                {
                    // ループを抜ける
                    break;
                }
                // 文字列が見つかった場合

                target = target.Remove(0, appearance_index + search_string.Length); // 発見した文字列まで削除
                count++;
            }

            return count;
        }

        /// <summary>
        ///     引数で受け取った文字列「src」内を検索し
        ///     「search_string」が始まる位置を返す。
        ///     もしマッチしなかった場合-1を返す。
        /// </summary>
        /// <param name="src">検索対象の文字列</param>
        /// <param name="search_string">検索文字列</param>
        /// <returns>検索文字列の開始位置</returns>
        private int searchIndex(string src, string search_string)
        {
            Match match = Regex.Match(src, $"{search_string}", RegexOptions.Singleline);
            if(match.Success)
            {
                return match.Index;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        ///     開始文字列の出現回数
        /// </summary>
        public int StartStringCount
        {
            get
            {
                return this.start_string_count;
            }
        }

        /// <summary>
        ///     終了文字列の出現回数
        /// </summary>
        public int EndStringCount
        {
            get
            {
                return this.end_string_count;
            }
        }
    }
}
