using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_Verlinkte_List_Generisch
{
    class FindStringSelector : ISelector
    {
        public bool Select(object obj)
        {
            return obj is string s;
        }
    }

}
