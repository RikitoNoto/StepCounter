using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Entry
{
    public class FileExplorer_c
    {

        public static bool Explore(string path, Func<string, bool> callback_func)
        {
            var dir_names = from dirs
                            in Directory.EnumerateDirectories(path, "", SearchOption.AllDirectories)
                            select dirs;

            FileExplorer_c.ExploreFile(path, callback_func); // 同じ階層のディレクトリを探索

            foreach(string dir_name in dir_names)
            {
                FileExplorer_c.ExploreFile(dir_name, callback_func);
            }
            return false;
        }

        private static bool ExploreFile(string path, Func<string, bool> callback_func)
        {
            var file_names =    from files
                                in Directory.EnumerateFiles(path)
                                select files;


            if (callback_func != null)
            {
                foreach (string file_name in file_names)
                {
                    callback_func(file_name);
                }

            }

            return false;
        }

    }
}
