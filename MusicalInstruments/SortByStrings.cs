using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public class SortByStrings: IComparer<Guitar>
    {
        public int Compare(Guitar? x, Guitar? y)
        {
            return x.StringCount.CompareTo(y.StringCount);
        }

    }
}
