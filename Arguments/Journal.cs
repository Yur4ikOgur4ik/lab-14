using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arguments
{
    public class Journal
    {
        public List<JournalEntry> elements = new List<JournalEntry>(); // Все записи

        public void CollectionCount(object source, CollectionHandlerEventArgs args) // Измеение количества объектов - сигнатура
        {
            string collectionName = args.Name;
            string changeType = args.ChangeType;
            string objectData = args.ChangedObject.ToString();

            var position = new JournalEntry(collectionName, changeType, objectData);
            elements.Add(position);
        }

        public void CollectionChanged(object source, CollectionHandlerEventArgs args) // Изменение объекта
        {
            string collectionName = args.Name;
            string changeType = args.ChangeType;
            string objectData = args.ChangedObject.ToString();

            var position = new JournalEntry(collectionName, changeType, objectData);
            elements.Add(position);
        }

        public override string ToString()
        {
            if (elements.Count == 0)
            {
                return "Журнал пуст";
            }
            else
            {
                return string.Join("\n", elements);
            }
        }

        public List<JournalEntry> GetEntries() => elements;
    }
}

