using Collections;
using LW11;
using MusicalInstruments;
using LW11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class LinqTests
    {
        private SortedDictionary<string, List<MusicalInstrument>> participants;
        private MyCollection<MusicalInstrument> instrumentCollection;

        [TestInitialize]
        public void Setup()
        {
            // Заполняем участников
            participants = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["Jora"] = new List<MusicalInstrument>
                {
                    new Piano("Piano", 100, "Octave", 88),
                    new Guitar("Acoustic", 101, 6)
                },
                ["Grisha"] = new List<MusicalInstrument>
                {
                    new Guitar("Bass", 102, 4)
                },
                ["Yasha"] = new List<MusicalInstrument>
                {
                    new Piano("Digital", 103, "Digital", 76)
                }
            };

            // Заполняем коллекцию
            instrumentCollection = new MyCollection<MusicalInstrument>(5);
            instrumentCollection.Add(new Guitar("Stratocaster", 104, 6));
            instrumentCollection.Add(new Guitar("Les Paul", 105, 7));
            instrumentCollection.Add(new Piano("Grand", 106, "C", 90));
        }

        #region Участники с гитарами

        [TestMethod]
        public void AllGuitarPlayersForeach_ReturnsCorrectNames()
        {
            var result = Methods.AllGuitarPlayersForeach(participants);
            CollectionAssert.Contains(result, "Jora");
            CollectionAssert.Contains(result, "Grisha");
            CollectionAssert.DoesNotContain(result, "Yasha");
        }

        [TestMethod]
        public void AllGuitarPlayersLINQ_ReturnsCorrectNames()
        {
            var result = Methods.AllGuitarPlayersLINQ(participants).ToList();
            CollectionAssert.Contains(result, "Jora");
            CollectionAssert.Contains(result, "Grisha");
            CollectionAssert.DoesNotContain(result, "Yasha");
        }

        [TestMethod]
        public void AllGuitarPlayersExtension_ReturnsCorrectNames()
        {
            var result = Methods.AllGuitarPlayersExtension(participants).ToList();
            CollectionAssert.Contains(result, "Jora");
            CollectionAssert.Contains(result, "Grisha");
            CollectionAssert.DoesNotContain(result, "Yasha");
        }

        #endregion

        #region Пересечение списков

        [TestMethod]
        public void IntersectInstruments_ReturnsCommonInstruments()
        {
            var list1 = new List<MusicalInstrument>
            {
                new Guitar("Classic", 100, 6),
                new Piano("Piano", 101, "C", 88)
            };
            var list2 = new List<MusicalInstrument>
            {
                new Guitar("Classic", 100, 6),
                new Piano("Upright", 102, "Minor", 76)
            };

            var result = Methods.IntersectInstruments(list1, list2).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Classic", result[0].Name);
        }

        [TestMethod]
        public void IntersectInstruments2_ReturnsCommonInstruments()
        {
            var list1 = new List<MusicalInstrument>
            {
                new Guitar("Classic", 100, 6),
                new Piano("Piano", 101, "C", 88)
            };
            var list2 = new List<MusicalInstrument>
            {
                new Guitar("Classic", 100, 6),
                new Piano("Upright", 102, "Minor", 76)
            };

            var result = Methods.IntersectInstruments2(list1, list2).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Classic", result[0].Name);
        }

        #endregion

        #region Минимальное количество струн

        [TestMethod]
        public void GetMinStringCount_ReturnsMinimum()
        {
            var participantsWithGuitars = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["A"] = new List<MusicalInstrument> { new Guitar("Low", 100, 4) },
                ["B"] = new List<MusicalInstrument> { new Guitar("High", 101, 8) }
            };

            int min = participantsWithGuitars.GetMinStringCount();
            Assert.AreEqual(4, min);
        }

        [TestMethod]
        public void GetGuitarWithMinStringsViaQuery_ReturnsMinimum()
        {
            var participantsWithGuitars = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["A"] = new List<MusicalInstrument> { new Guitar("Low", 100, 4) },
                ["B"] = new List<MusicalInstrument> { new Guitar("High", 101, 8) }
            };

            int min = participantsWithGuitars.GetGuitarWithMinStringsViaQuery();
            Assert.AreEqual(4, min);
        }

        #endregion

        #region Группировка

        [TestMethod]
        public void GroupInstrumentsByTypeQuery_ReturnsCorrectGroups()
        {
            var result = Methods.GroupInstrumentsByTypeQuery(participants).ToList();

            Assert.IsTrue(result.Any(g => g.Key == "Guitar"));
            Assert.IsTrue(result.Any(g => g.Key == "Piano"));
        }

        [TestMethod]
        public void GroupInstrumentsByType_ReturnsCorrectGroups()
        {
            var result = Methods.GroupInstrumentsByType(participants).ToList();

            Assert.IsTrue(result.Any(g => g.Key == "Guitar"));
            Assert.IsTrue(result.Any(g => g.Key == "Piano"));
        }

        #endregion

        #region Топ гитар по количеству струн

        [TestMethod]
        public void TopInstrumentsByStringCount_ReturnsOrdered()
        {
            var result = Methods.TopInstrumentsByStringCount(participants, 2).ToList();

            var first = (dynamic)result[0];
            var second = (dynamic)result[1];

            Assert.IsTrue(first.StringCount >= second.StringCount);
        }

        [TestMethod]
        public void TopInstrumentsByStringCountMethod_ReturnsOrdered()
        {
            var result = Methods.TopInstrumentsByStringCountMethod(participants, 2).ToList();

            var first = (dynamic)result[0];
            var second = (dynamic)result[1];

            Assert.IsTrue(first.StringCount >= second.StringCount);
        }

        #endregion

        #region Работа с MyCollection

        [TestMethod]
        public void GetGuitarsWithLinq_ReturnsOnlyGuitars()
        {
            var result = instrumentCollection.GetGuitarsWithLinq().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(i => i is Guitar));
        }

        [TestMethod]
        public void GetGuitarsWithExtension_ReturnsOnlyGuitars()
        {
            var result = instrumentCollection.GetGuitarsWithExtension().ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(i => i is Guitar));
        }

        [TestMethod]
        public void TotalStringsLINQ_CorrectSum()
        {
            var result = instrumentCollection.TotalStringsLINQ();
            Assert.AreEqual(6 + 7, result); // Stratocaster (6), Les Paul (7)
        }

        [TestMethod]
        public void MaxStringCountExtension_CorrectMax()
        {
            var result = instrumentCollection.MaxStringCountExtension();
            Assert.AreEqual(7, result);
        }

        #endregion

        #region Исключительные ситуации

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetMinStringCount_ThrowsWhenNoGuitars()
        {
            var emptyParticipants = new SortedDictionary<string, List<MusicalInstrument>>();
            emptyParticipants["NoGuitar"] = new List<MusicalInstrument> { new Piano() };
            emptyParticipants.GetMinStringCount();
        }

        #endregion
    }
}
