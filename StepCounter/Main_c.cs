using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;

namespace StepCounter
{
    public class Main_c
    {
        // オプション構造体
        public struct OPTION_INFO_S
        {
            public OPTION_INFO_S(OPTION_ID_E id, string name, string key, string description, CommandOptionType type)
            {
                this.m_id = id;
                this.m_name = name;
                this.m_key = key;
                this.m_description = description;
                this.m_type = type;
            }
            private OPTION_ID_E m_id;           // オプションID
            private string m_name;              // オプション名
            private string m_key;               // オプションキー(実際のコマンドラインで呼び出す文字(-h等))
            private string m_description;       // オプションの詳細
            private CommandOptionType m_type;   // オプションのタイプ

            // メンバーのアクセサ
            public OPTION_ID_E Id { get { return this.m_id; } }
            public string Name{ get { return this.m_name; } }
            public string Key { get { return this.m_key; } }
            public string Description { get { return this.m_description; } }
            public CommandOptionType Type { get { return this.m_type; } }

        }

        public enum OPTION_ID_E
        {
            OPTION_ID_START_STRING,
            OPTION_ID_END_STRING,
            OPTION_COUNT
        }

        static private Dictionary<OPTION_ID_E, OPTION_INFO_S> m_option_infos = new Dictionary<OPTION_ID_E, OPTION_INFO_S>()
        {
            // 開始文字列 オプション
            {OPTION_ID_E.OPTION_ID_START_STRING,  new OPTION_INFO_S(    OPTION_ID_E.OPTION_ID_START_STRING,
                                                                        "start_string",
                                                                        "-ss|--startstring",
                                                                        "検索の開始トリガとする文字列",
                                                                        CommandOptionType.SingleValue) },
            // 終了文字列 オプション
            {OPTION_ID_E.OPTION_ID_END_STRING,  new OPTION_INFO_S(      OPTION_ID_E.OPTION_ID_END_STRING,
                                                                        "end_string",
                                                                        "-es|--endstring",
                                                                        "検索の終了トリガとする文字列",
                                                                        CommandOptionType.SingleValue) },

        };

        static private Dictionary<OPTION_ID_E, CommandOption> m_option_instances = null;


        private const string OPTION_KEY_HELP = "-?|-h|--help";
        static private string[] m_args = null;
        static private Dictionary<OPTION_ID_E, string[]> m_option_args = null;

        public static int Main(string[] args)
        {
            Main_c.m_args = new string[args.Length];    // 受け取った引数の個数で配列を初期化
            args.CopyTo(Main_c.m_args, 0);              // 引数をコピー

            Main_c.m_option_args = new Dictionary<OPTION_ID_E, string[]>();             // オプション引数を初期化
            Main_c.m_option_instances = new Dictionary<OPTION_ID_E, CommandOption>();   // オプションインスタンス辞書を初期化

            CommandLineApplication app = new CommandLineApplication(throwOnUnexpectedArg: false); // アプリインスタンスの作成
            Main_c.RegistOptions(app); // オプションインスタンスの登録

            // アプリの処理
            app.OnExecute(() =>
            {
                // オプションの引数を変数に登録
                foreach(OPTION_ID_E id in Main_c.m_option_instances.Keys)
                {
                    CommandOption option = Main_c.m_option_instances[id];
                    // オプションが引数を持っていたら
                    if(option.HasValue())
                    {
                        Main_c.m_option_args.Add(id, new string[1] { option.Value() }); // 引数を追加
                    }
                }

                return 0;
            });

            app.Execute(args);
            return 0;
        }

        /// <summary>
        ///     登録されているオプション情報から、オプションインスタンスの登録を行う
        /// </summary>
        /// <param name="app">オプションを登録するアプリインスタンス</param>
        static private void RegistOptions(CommandLineApplication app)
        {
            // オプション情報をループ
            foreach(OPTION_ID_E id in Main_c.m_option_infos.Keys)
            {
                OPTION_INFO_S info = Main_c.m_option_infos[id]; // オプション情報

                // オプションインスタンス作成
                CommandOption option = app.Option(info.Key, info.Description, info.Type);
                option.LongName = info.Name;    // オプション名の登録

                Main_c.m_option_instances.Add(id, option);  // 辞書へインスタンスの登録
            }
        }


        /// <summary>
        ///     実行時の引数
        /// </summary>
        public static string[] Args
        {
            get
            {
                return Main_c.m_args;
            }
        }

        /// <summary>
        ///     オプション引数
        /// </summary>
        public static Dictionary<OPTION_ID_E, string[]> OptionArgs
        {
            get
            {
                return Main_c.m_option_args;
            }
        }
    }
}
