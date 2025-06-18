using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class HashPoint<T>
    {
        public T? Data { get; set; }
        public bool IsDeleted { get; set; } //для отметки удаленных

        public HashPoint()
        {
            Data = default(T);
            IsDeleted = false;
        }

        public HashPoint(T info)
        {
            Data = info;
            IsDeleted = false;
        }

        public override string ToString()
        {
            return Data?.ToString() ?? "null";
        }
    }
}
