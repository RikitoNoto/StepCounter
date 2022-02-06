using System;
using System.Collections.Generic;
using System.IO;

namespace Entry
{
    public class FileExplorer_c
    {
        public static bool Explore(string path)
        {
            Directory.EnumerateFiles(path);
            return false;
        }
    }
}
