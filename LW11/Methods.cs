using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collections;
using MusicalInstruments;

namespace LW11
{
    public static class Methods
    {
        public static List<string> AllGuitarPlayersForeach(SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            List<string> guitarPlayers = new List<string>();

            foreach (var entry in participants)
            {
                foreach (var instrument in entry.Value)
                {
                    if (instrument is Guitar)
                    {
                        guitarPlayers.Add(entry.Key);
                        break; // выходим из цикла по инструментам, если нашли гитару
                    }
                }
            }

            return guitarPlayers;
        }
        public static IEnumerable<string> AllGuitarPlayersLINQ(SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            var guitarPlayers = from entry in participants//linq zapros
                                where entry.Value.Any(instr => instr is Guitar)
                                select entry.Key;
            return guitarPlayers;
        }

        public static IEnumerable<string> AllGuitarPlayersExtension(SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            var guitarPlayers = participants//rashirenie
                .Where(entry => entry.Value.Any(instr => instr is Guitar))
                .Select(entry => entry.Key);
            return guitarPlayers;
        }

        public static IEnumerable<MusicalInstrument> IntersectInstruments(List<MusicalInstrument> instr1, List<MusicalInstrument> instr2) 
        {
            return instr1.Intersect(instr2);//rash
        }
        
        public static IEnumerable<MusicalInstrument> IntersectInstruments2(List<MusicalInstrument> instr1, List<MusicalInstrument> instr2)
        {
            var common = from instrument in instr1//linq
                         where instr2.Contains(instrument)
                         select instrument;

            return common;
        }

        public static int GetMinStringCount(this SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            if (participants == null || participants.Count == 0)
                throw new ArgumentException("Participants dictionary is empty.");

            var allGuitars = participants
                .SelectMany(p => p.Value.OfType<Guitar>()) // выбираем только гитары
                .ToList();

            if (!allGuitars.Any())
                throw new InvalidOperationException("No guitars found among participants.");

            return allGuitars.Min(g => g.StringCount); // находим минимальное значение StringCount
        }

        public static int GetGuitarWithMinStringsViaQuery(this SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            if (participants == null || participants.Count == 0)
                throw new ArgumentException("Participants dictionary is empty.");

            var allGuitars = from participant in participants
                             from instrument in participant.Value
                             where instrument is Guitar
                             select (Guitar)instrument;

            var guitarList = allGuitars.ToList();

            if (!guitarList.Any())
                throw new InvalidOperationException("No guitars found among participants.");

            int minStringCount = guitarList.Min(g => g.StringCount);//min num of strings

            return minStringCount;
        }

        public static IEnumerable<IGrouping<string, MusicalInstrument>> GroupInstrumentsByTypeQuery(this SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            var grouped = from participant in participants
                          from instrument in participant.Value
                          group instrument by instrument.GetType().Name;

            return grouped;
        }

        public static IEnumerable<IGrouping<string, MusicalInstrument>> GroupInstrumentsByType(this SortedDictionary<string, List<MusicalInstrument>> participants)
        {
            return participants
                .SelectMany(p => p.Value) // объединяем все списки инструментов в один
                .GroupBy(i => i.GetType().Name); // группируем по типу
        }

        public static IEnumerable<object> TopInstrumentsByStringCount(this SortedDictionary<string, List<MusicalInstrument>> participants, int count = 3)
        {
            var result = (from participant in participants
                          from instrument in participant.Value.OfType<Guitar>()
                          orderby instrument.StringCount descending
                          select new
                          {
                              Name = instrument.Name,
                              StringCount = instrument.StringCount,
                              Participant = participant.Key
                          }).Take(count);

            return result;
        }
        public static IEnumerable<object> TopInstrumentsByStringCountMethod(this SortedDictionary<string, List<MusicalInstrument>> participants, int count = 5)
        {
            return participants
                .SelectMany(p => p.Value.OfType<Guitar>().Select(g => new
                {
                    g.Name,
                    g.StringCount,
                    Participant = p.Key
                }))
                .OrderByDescending(i => i.StringCount)
                .Take(count);
        }

        // part 2
        //all guitars
        public static IEnumerable<Guitar> GetGuitarsWithLinq(this MyCollection<MusicalInstrument> collection)
        {
            var query = from instrument in collection
                        where instrument is Guitar
                        select (Guitar)instrument;

            return query;
        }

        public static IEnumerable<Guitar> GetGuitarsWithExtension(this MyCollection<MusicalInstrument> collection)
        {
            return collection
                .Where(instr => instr is Guitar)
                .Cast<Guitar>();
        }

        public static int GetTotalInstrumentsWithExtension(this MyCollection<MusicalInstrument> collection)
        {
            return collection.Count();
        }

        public static int GetGuitarCountWithLinq(this MyCollection<MusicalInstrument> collection)
        {
            var count = (from instrument in collection
                         where instrument is Guitar
                         select instrument).Count();

            return count;
        }

        public static int TotalStringsLINQ(this MyCollection<MusicalInstrument> collection)
        {
            var guitars = from instrument in collection
                          where instrument is Guitar
                          select (Guitar)instrument;

            return guitars.Sum(g => g.StringCount);
        }

        public static int TotalStringsExtension(this MyCollection<MusicalInstrument> collection)
        {
            return collection
                .OfType<Guitar>()
                .Sum(g => g.StringCount);
        }

        public static int MaxStringCountLINQ(this MyCollection<MusicalInstrument> collection)
        {
            var guitars = from instrument in collection
                          where instrument is Guitar
                          select (Guitar)instrument;

            return guitars.Any() ? guitars.Max(g => g.StringCount) : 0;
        }

        public static int MaxStringCountExtension(this MyCollection<MusicalInstrument> collection)
        {
            return collection
                .OfType<Guitar>()
                .Max(g => g.StringCount);
        }

    }
}
