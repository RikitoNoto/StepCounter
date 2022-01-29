using System;
using System.Collections.Generic;
using System.Text;

namespace Filter
{
    public interface Filter_if
    {
        string[] Filtering(string src, string start_string, string end_string);
    }
}
