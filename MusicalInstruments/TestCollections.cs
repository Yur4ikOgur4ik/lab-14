using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public class TestCollections
    {
        private Queue<Piano> queuePianos;
        private Queue<string> queueStrings;
        private Dictionary<MusicalInstrument, Piano> dictionaryInstrumentToPiano;
        private Dictionary<string, Piano> dictionaryStringToPiano;

        public TestCollections(int count)
        {
            queuePianos = new Queue<Piano>();
            queueStrings = new Queue<string>();
            dictionaryInstrumentToPiano = new Dictionary<MusicalInstrument, Piano>();
            dictionaryStringToPiano = new Dictionary<string, Piano>();

            InitializeCollections(count);
        }

        public Piano first, middle, last, noexist;
        private void InitializeCollections(int size)
        {
            for (int i = 0; i < size; i++)
            {
                try
                {
                    Piano piano = new Piano();
                    piano.RandomInit();
                    piano.Name = piano.Name + i.ToString();
                    dictionaryInstrumentToPiano.Add(piano.GetBase, piano);
                    dictionaryStringToPiano.Add(piano.GetBase.ToString(), piano);
                    queuePianos.Enqueue(piano);
                    queueStrings.Enqueue(piano.ToString());
                    if (i == 0)
                        first = (Piano)piano.Clone();
                    else if (i == size / 2)
                    {
                        middle = (Piano)piano.Clone(); // Clone the middle piano
                    }
                    else if (i == size - 1)
                    {
                        last = (Piano)piano.Clone(); // Clone the last piano
                    }
                }
                catch 
                {
                    i--;
                }

            }
            noexist = new Piano("AbobusMaximusRexMisterPi", 12, "Digital", 88);
        }

        // Для очереди Piano
        public void FindItemInQueue(Piano item, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Queue<Piano> {message} element ");
            sw.Restart();
            bool ok = queuePianos.Contains(item);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }

        // Для очереди строк
        public void FindItemInStringQueue(string item, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Queue<string> {message} element ");
            sw.Restart();
            bool ok = queueStrings.Contains(item);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }

        // Для словаря с ключами MusicalInstrument
        public void FindItemInInstrumentDictionary(MusicalInstrument key, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Dictionary<MusicalInstrument, Piano> {message} key ");
            sw.Restart();
            bool ok = dictionaryInstrumentToPiano.ContainsKey(key);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }

        // Для словаря с ключами string
        public void FindItemInStringDictionary(string key, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Dictionary<string, Piano> {message} key ");
            sw.Restart();
            bool ok = dictionaryStringToPiano.ContainsKey(key);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }

        // Поиск по значению в словарях (медленнее)
        public void FindValueInInstrumentDictionary(Piano value, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Dictionary<MusicalInstrument, Piano> {message} value ");
            sw.Restart();
            bool ok = dictionaryInstrumentToPiano.ContainsValue(value);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }

        public void FindValueInStringDictionary(Piano value, string message)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.Write($"In Dictionary<string, Piano> {message} value ");
            sw.Restart();
            bool ok = dictionaryStringToPiano.ContainsValue(value);
            sw.Stop();
            if (ok)
                Console.Write("Found ");
            else
                Console.Write("Not Found ");
            Console.WriteLine($"in {sw.ElapsedTicks} ticks");
        }




    }
}
