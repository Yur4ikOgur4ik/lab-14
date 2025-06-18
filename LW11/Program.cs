using System;
using System.Collections.Generic;
using System.Linq;
using MusicalInstruments;
using Collections;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Xml.Serialization;

namespace LW11
{
    public class Program
    {
        public static SortedDictionary<string, List<MusicalInstrument>> participants;
        public static MyCollection<MusicalInstrument> instrumentCollection;

        public static void Main()
        {
            participants = new SortedDictionary<string, List<MusicalInstrument>>();
            string[] names = { "Jora", "Yasha", "Grisha", "Innokentiy", "Sergey", "Aleksandr" };
            Random rnd = new Random();

            // Инициализируем участников
            foreach (var name in names)
            {
                int count = rnd.Next(1, 4);
                var instruments = new List<MusicalInstrument>();

                for (int i = 0; i < count; i++)
                {
                    instruments.Add(ValidInput.CreateRandomInstr());
                }

                participants[name] = instruments;
            }

            
            List<MusicalInstrument> list1 = new List<MusicalInstrument>();
            List<MusicalInstrument> list2 = new List<MusicalInstrument>();
            for (int i = 0;i<10;i++)
            {
                list1.Add(ValidInput.CreateRandomInstr());
                list2.Add(ValidInput.CreateRandomInstr());
            }

            do 
            {
                Console.WriteLine("=======Menu=======");
                Console.WriteLine("1. Show all participants");
                Console.WriteLine("2. Where linq + extension");
                Console.WriteLine("3. Compare for and linq + extensions");
                Console.WriteLine("4. Intersection linq + extension");
                Console.WriteLine("5. Min");


                int choice = ValidInput.GetInt();

                switch (choice) 
                {
                    case 1:
                        ShowAllParticipants(); break;
                    case 2:
                        ShowGuitarPlayersComparison(); break;
                    case 3:
                        ComparePerformance(participants);
                        break;
                    case 4:
                        Intersection(list1, list2 ); break;
                    case 5:
                        MinGuitar();
                        break;
                    case 6:
                        Grouping();
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }

            } while (true);
        }
        public static void ComparePerformance(SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            int iterations = 10_000;

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var result = Methods.AllGuitarPlayersLINQ(participants).ToList();
            }
            watch.Stop();
            long linqTime = watch.ElapsedMilliseconds;

            watch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var result = Methods.AllGuitarPlayersExtension(participants).ToList();
            }
            watch.Stop();
            long extensionTime = watch.ElapsedMilliseconds;

            watch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var result = Methods.AllGuitarPlayersForeach(participants);
            }
            watch.Stop();
            long foreachTime = watch.ElapsedMilliseconds;

            Console.WriteLine($"\n=== Performance Comparison ===");
            Console.WriteLine($"Method: AllGuitarPlayersLINQ - {linqTime} ms for {iterations} iterations");
            Console.WriteLine($"Method: AllGuitarPlayersExtension - {extensionTime} ms for {iterations} iterations");
            Console.WriteLine($"Method: AllGuitarPlayersForeach - {foreachTime} ms for {iterations} iterations");

            string fastest;
            if (linqTime <= extensionTime && linqTime <= foreachTime)
                fastest = "AllGuitarPlayersLINQ";
            else if (extensionTime <= linqTime && extensionTime <= foreachTime)
                fastest = "AllGuitarPlayersExtension";
            else
                fastest = "AllGuitarPlayersForeach";

            Console.WriteLine($"\nFastest method: {fastest}");
        }
        static void ShowAllParticipants()
        {
            Console.WriteLine("All Participants and Their Instruments:");
            foreach (var entry in participants)
            {
                Console.WriteLine($"Participant: {entry.Key}");
                foreach (var instr in entry.Value)
                {
                    Console.WriteLine($" - {instr.ToString()}");
                }
                Console.WriteLine(); // разделитель между участниками
            }
        }
        static void ShowGuitarPlayersComparison()
        {
            Console.WriteLine("\n=== Guitar Players ===");

            var linqResult = Methods.AllGuitarPlayersLINQ(participants);
            Console.WriteLine("Using LINQ Query Syntax:");
            if (linqResult.Any())
            {
                foreach (var name in linqResult)
                {
                    Console.WriteLine($" - {name}");
                }
            }
            else
            {
                Console.WriteLine("No guitar players found.");
            }

            var extensionResult = Methods.AllGuitarPlayersExtension(participants);
            Console.WriteLine("\nUsing Extension Methods (Method Syntax):");
            if (extensionResult.Any())
            {
                foreach (var name in extensionResult)
                {
                    Console.WriteLine($" - {name}");
                }
            }
            else
            {
                Console.WriteLine("No guitar players found.");
            }
        }
        static void PrintInstruments(List<MusicalInstrument> instruments)
        {
            if (instruments == null || instruments.Count == 0)
            {
                Console.WriteLine("No instruments found.");
                return;
            }

            foreach (var instr in instruments)
            {
                Console.WriteLine(instr);
            }
        }

        static public void Intersection(List<MusicalInstrument> list1, List<MusicalInstrument> list2)
        {
            Console.WriteLine("=== List 1 ===");
            PrintInstruments(list1);

            Console.WriteLine("\n=== List 2 ===");
            PrintInstruments(list2);

            Console.WriteLine("\n=== Common Instruments (Intersect) ===");
            var common1 = Methods.IntersectInstruments(list1, list2);
            PrintInstruments(common1.ToList());

            Console.WriteLine("\n=== Common Instruments (LINQ Query) ===");
            var common2 = Methods.IntersectInstruments2(list1, list2);
            PrintInstruments(common2.ToList());
        }

        static public void MinGuitar()
        {
            try
            {
                Console.WriteLine("\n=== Running GetMinStringCount ===");
                int min1 = participants.GetMinStringCount();
                Console.WriteLine($"Minimum number of strings: {min1}");

                Console.WriteLine("\n=== Running GetGuitarWithMinStringsViaQuery ===");
                int min2 = participants.GetGuitarWithMinStringsViaQuery();
                Console.WriteLine($"Minimum number of strings: {min2}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }

        private static void PrintGroups(IEnumerable<IGrouping<string, MusicalInstrument>> groups)
        {
            if (!groups.Any())
            {
                Console.WriteLine("No instruments found.");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"\nType: {group.Key} | Count: {group.Count()}");
                foreach (var instrument in group)
                {
                    Console.WriteLine($" - {instrument.Name}");
                }
            }
        }

        static public void Grouping()
        {
            Console.WriteLine("\n=== Grouped by Type (GroupInstrumentsByTypeQuery) ===");
            var groupedQuery = Methods.GroupInstrumentsByTypeQuery(participants);
            PrintGroups(groupedQuery);

            Console.WriteLine("\n=== Grouped by Type (GroupInstrumentsByType) ===");
            var groupedMethod = Methods.GroupInstrumentsByType(participants);
            PrintGroups(groupedMethod);
        }
    }
}