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
        public static MyCollection<MusicalInstrument> instrumentCollection = new MyCollection<MusicalInstrument>(10);

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
            for (int i = 0; i<10; i++)
            {
                instrumentCollection.Add(ValidInput.CreateRandomInstr());
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
                Console.WriteLine("6. Grouping");
                Console.WriteLine("7. Getting new type");
                Console.WriteLine("8. Collection where");
                Console.WriteLine("9. Collection Count");
                Console.WriteLine("10. Collection Max");
                Console.WriteLine("11. Collection Group by");


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
                    case 7:
                        TopGuitar();
                        break;
                    case 8:
                        DemonstrateGuitarSelection();
                        break;
                    case 9:
                        DemoCount();
                        break;
                    case 10:
                        DemonstrateTotalAndMaxStringCount();
                        break;
                    case 11:
                        DemonstrateGrouping();
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
        
        static public void TopGuitar()
        {
            Console.WriteLine($"\n=== Top {3} Instruments by String Count (Query Syntax) ===");
            var result1 = Methods.TopInstrumentsByStringCount(participants);
            PrintTopGuitars(result1);

            Console.WriteLine($"\n=== Top {3} Instruments by String Count (Method Syntax) ===");
            var result2 = Methods.TopInstrumentsByStringCountMethod(participants);
            PrintTopGuitars(result2);
        }
        private static void PrintTopGuitars(IEnumerable<object> topGuitars)
        {
            if (!topGuitars.Any())
            {
                Console.WriteLine("No guitars found.");
                return;
            }

            foreach (dynamic item in topGuitars)
            {
                Console.WriteLine($"Name: {item.Name}, Strings: {item.StringCount}, Participant: {item.Participant}");
            }
        }

        public static void DemonstrateGrouping()
        {



            Console.WriteLine("\n=== Grouped by Type (Query Syntax) ===");
            var result1 = instrumentCollection.GroupInstrumentsByTypeQuery();
            PrintGroups(result1);

            Console.WriteLine("\n=== Grouped by Type (Method Syntax) ===");
            var result2 = instrumentCollection.GroupInstrumentsByType();
            PrintGroups(result2);
        }

        public static void DemoCount()
        {
            int totalInstruments = instrumentCollection.GetTotalInstrumentsWithExtension();
            Console.WriteLine($"Total instruments in collection: {totalInstruments}");

            int totalGuitars = instrumentCollection.GetGuitarCountWithLinq();
            Console.WriteLine($"Number of guitars (LINQ): {totalGuitars}");
        }

        public static void DemonstrateGuitarSelection()
        {
            Console.WriteLine("\n=== Guitars selected via LINQ Query Syntax ===");
            var linqResult = instrumentCollection.GetGuitarsWithLinq();
            if (linqResult.Any())
            {
                foreach (var guitar in linqResult)
                {
                    Console.WriteLine(guitar);
                }
            }
            else
            {
                Console.WriteLine("No guitars found.");
            }

            Console.WriteLine("\n=== Guitars selected via Extension Methods ===");
            var extensionResult = instrumentCollection.GetGuitarsWithExtension();
            if (extensionResult.Any())
            {
                foreach (var guitar in extensionResult)
                {
                    Console.WriteLine(guitar);
                }
            }
            else
            {
                Console.WriteLine("No guitars found.");
            }

        }

        public static void DemonstrateTotalAndMaxStringCount()
        {

            // Вызываем методы агрегации
            Console.WriteLine("\n=== Aggregation Results ===");

            int totalStrings = instrumentCollection.TotalStringsExtension();
            Console.WriteLine($"Total strings in all guitars: {totalStrings}");

            int maxStringCount = instrumentCollection.MaxStringCountLINQ();
            Console.WriteLine($"Maximum string count among guitars: {maxStringCount}");
        }
    }
}