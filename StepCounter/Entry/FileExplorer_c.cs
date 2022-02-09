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
            var file_names = from files 
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
