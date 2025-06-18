using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arguments
{
    public class JournalEntry
    {
        public string CollectionName { get; }
        public string ChangeType { get; }
        public string ObjectData { get; }

        public JournalEntry(string collectionName, string changeType, string objectData)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ObjectData = objectData;
        }

        public override string ToString()
        {
            return $"[{CollectionName}] {ChangeType}: {ObjectData}";
        }
    }
}
