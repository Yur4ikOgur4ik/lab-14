using Arguments;
using Collections;
using LW11;
using MusicalInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class Lab13Tests
    {
        private MyObservableCollection<MusicalInstrument> collection;
        private Journal journal;

        [TestInitialize]
        public void Initialize()
        {
            collection = new MyObservableCollection<MusicalInstrument>();
            collection.Name = "InstrumentCollection";
            journal = new Journal();

            // Подписываемся на события коллекции
            collection.CollectionCountChanged += journal.CollectionCount;
            collection.CollectionReferenceChanged += journal.CollectionChanged;
        }

        [TestMethod]
        public void Add_Item_ShouldTriggerCountEvent()
        {
            // Arrange
            var instrument = new Piano("Grand Piano", 101, "Octave", 88);

            // Act
            collection.Add(instrument);

            // Assert
            Assert.AreEqual(1, journal.GetEntries().Count);
            var entry = journal.GetEntries()[0];
            Assert.AreEqual("InstrumentCollection", entry.CollectionName);
            Assert.AreEqual("Добавлен элемент", entry.ChangeType);
            Assert.IsTrue(entry.ObjectData.Contains("Grand Piano"));
        }

        [TestMethod]
        public void Remove_Item_ShouldTriggerCountEvent()
        {
            // Arrange
            var instrument = new Piano("Grand Piano", 101, "Octave", 88);
            collection.Add(instrument);

            // Act
            bool removed = collection.Remove(instrument);

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(2, journal.GetEntries().Count); // add + remove
            var entry = journal.GetEntries()[1];
            Assert.AreEqual("Удалён элемент", entry.ChangeType);
            Assert.IsTrue(entry.ObjectData.Contains("Grand Piano"));
        }

        [TestMethod]
        public void Replace_Item_ShouldTriggerChangeEvent()
        {
            // Arrange
            var oldItem = new Piano("Old Piano", 101, "Octave", 88);
            var newItem = new Piano("New Piano", 102, "Digital", 76);
            collection.Add(oldItem);

            // Act
            collection[oldItem] = newItem;

            // Assert
            Assert.AreEqual(4, journal.GetEntries().Count); // add + replace
            var entry = journal.GetEntries()[1];
            Assert.AreEqual("Удалён элемент", entry.ChangeType);
            
        }

        [TestMethod]
        public void Replace_NonExistentItem_ThrowsKeyNotFoundException()
        {
            // Arrange
            var oldItem = new Piano("Old Piano", 101, "Octave", 88);
            var newItem = new Piano("New Piano", 102, "Digital", 76);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => collection[oldItem] = newItem);
        }

        [TestMethod]
        public void Indexer_Get_ReturnsCorrectItem()
        {
            // Arrange
            var item = new Piano("Acoustic Guitar", 201, "Scale", 45);
            collection.Add(item);

            // Act
            var result = collection[item];

            // Assert
            Assert.AreSame(item, result);
        }

        [TestMethod]
        public void Journal_ContainsAllEvents()
        {
            // Arrange
            var piano = new Piano("Piano", 1, "Octave", 88);
            var guitar = new Piano("Guitar", 2, "Scale", 45);

            // Act
            collection.Add(piano);
            collection.Add(guitar);
            collection[guitar] = new Piano("Bass", 3, "C", 36);
            collection.Remove(piano);

            // Assert
            var entries = journal.GetEntries();
            Assert.AreEqual(6, entries.Count);
            Assert.AreEqual("Добавлен элемент", entries[0].ChangeType);
            Assert.AreEqual("Добавлен элемент", entries[1].ChangeType);
            Assert.AreEqual("Элемент заменён", entries[4].ChangeType);
            Assert.AreEqual("Удалён элемент", entries[5].ChangeType);
        }

        [TestMethod]
        public void Journal_ToString_WithEntries_ReturnsFormattedString()
        {
            // Arrange
            var journal = new Journal();
            var args = new CollectionHandlerEventArgs("TestCollection", "Добавлен элемент", "Piano");
            journal.CollectionCount(null, args);

            // Act
            string result = journal.ToString();

            // Assert
            string expected = $"[TestCollection] Добавлен элемент: Piano";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToString_WhenJournalIsEmpty_ReturnsEmptyMessage()
        {
            // Arrange
            var journal = new Journal();

            // Act
            string result = journal.ToString();

            // Assert
            Assert.AreEqual("Журнал пуст", result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Indexer_Get_WhenContainsTrueButItemNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var collection = new MyObservableCollection<MusicalInstrument>();
            var piano = new TestPiano { Name = "BrokenPiano" };

            collection.Add(piano); // Добавляем элемент

            // Act
            var result = collection[piano]; // Вызываем индексатор

            // Assert handled by ExpectedException
        }
        public class TestPiano : Piano
        {
            public override bool Equals(object obj)
            {
                return false; // <-- Заставляем Equals() всегда возвращать false
            }
        }

        [TestMethod]
        public void GetTotalInstrumentsWithExtension_ReturnsCorrectCount()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Piano("Grand", 101, "Octave", 88));
            collection.Add(new Guitar("Bass", 102, 4));

            // Act
            int result = collection.GetTotalInstrumentsWithExtension();

            // Assert
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void GetGuitarCountWithLinq_ReturnsCorrectGuitarCount()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Piano("Grand", 101, "Octave", 88));
            collection.Add(new Guitar("Bass", 102, 4));

            // Act
            int result = collection.GetGuitarCountWithLinq();

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void TotalStringsExtension_SumsAllGuitarStrings()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Guitar("Bass", 102, 4));
            collection.Add(new Piano("Grand", 101, "Octave", 88));

            // Act
            int totalStrings = collection.TotalStringsExtension();

            // Assert
            Assert.AreEqual(10, totalStrings); // 6 + 4
        }

        [TestMethod]
        public void MaxStringCountLINQ_ReturnsMaxStringCount()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Guitar("Bass", 102, 4));
            collection.Add(new Guitar("Les Paul", 103, 7));

            // Act
            int maxStrings = collection.MaxStringCountLINQ();

            // Assert
            Assert.AreEqual(7, maxStrings);
        }

        [TestMethod]
        public void MaxStringCountLINQ_NoGuitars_ReturnsZero()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Piano("Piano", 101, "Octave", 88));
            collection.Add(new MusicalInstrument("UraNetToi", 102));

            // Act
            int maxStrings = collection.MaxStringCountLINQ();

            // Assert
            Assert.AreEqual(0, maxStrings);
        }

        [TestMethod]
        public void GroupInstrumentsByTypeQuery_GroupsCorrectly()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Piano("Grand", 101, "Octave", 88));
            collection.Add(new Guitar("Bass", 102, 4));
            collection.Add(new Piano("Digital", 103, "Digital", 76));
            collection.Add(new MusicalInstrument("UraNetToi", 104));

            // Act
            var groups = collection.GroupInstrumentsByTypeQuery().ToList();

            // Assert
            Assert.IsTrue(groups.Any(g => g.Key == "Guitar"));
            Assert.IsTrue(groups.Any(g => g.Key == "Piano"));
            Assert.IsTrue(groups.Any(g => g.Key == "MusicalInstrument"));

            var guitarGroup = groups.First(g => g.Key == "Guitar");
            Assert.AreEqual(2, guitarGroup.Count());

            var pianoGroup = groups.First(g => g.Key == "Piano");
            Assert.AreEqual(2, pianoGroup.Count());

            var instrumentGroup = groups.First(g => g.Key == "MusicalInstrument");
            Assert.AreEqual(6, instrumentGroup.Count());
        }

        [TestMethod]
        public void GroupInstrumentsByType_MethodSyntax_GroupsCorrectly()
        {
            // Arrange
            var collection = new MyCollection<MusicalInstrument>(5);
            collection.Add(new Guitar("Stratocaster", 100, 6));
            collection.Add(new Piano("Grand", 101, "Octave", 88));
            collection.Add(new Guitar("Bass", 102, 4));
            collection.Add(new Piano("Digital", 103, "Digital", 76));
            collection.Add(new MusicalInstrument("UraNetToi", 104));

            // Act
            var groups = collection.GroupInstrumentsByType().ToList();

            // Assert
            Assert.IsTrue(groups.Any(g => g.Key == "Guitar"));
            Assert.IsTrue(groups.Any(g => g.Key == "Piano"));
            Assert.IsTrue(groups.Any(g => g.Key == "MusicalInstrument"));

            var guitarGroup = groups.First(g => g.Key == "Guitar");
            Assert.AreEqual(2, guitarGroup.Count());

            var pianoGroup = groups.First(g => g.Key == "Piano");
            Assert.AreEqual(2, pianoGroup.Count());

            var instrumentGroup = groups.First(g => g.Key == "MusicalInstrument");
            Assert.AreEqual(6, instrumentGroup.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetGuitarWithMinStringsViaQuery_NullParticipants_ThrowsArgumentException()
        {
            // Arrange
            SortedDictionary<string, List<MusicalInstrument>> participants = null;

            // Act
            participants.GetGuitarWithMinStringsViaQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetGuitarWithMinStringsViaQuery_EmptyParticipants_ThrowsArgumentException()
        {
            // Arrange
            var participants = new SortedDictionary<string, List<MusicalInstrument>>();

            // Act
            participants.GetGuitarWithMinStringsViaQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetGuitarWithMinStringsViaQuery_NoGuitars_ThrowsInvalidOperationException()
        {
            // Arrange
            var participants = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["Jora"] = new List<MusicalInstrument> { new Piano("Grand", 100, "C", 88) },
                ["Grisha"] = new List<MusicalInstrument> { new MusicalInstrument("UraNetToi", 101) }
            };

            // Act
            participants.GetGuitarWithMinStringsViaQuery();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMinStringCount_NullParticipants_ThrowsArgumentException()
        {
            // Arrange
            SortedDictionary<string, List<MusicalInstrument>> participants = null;

            // Act
            participants.GetMinStringCount();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMinStringCount_EmptyParticipants_ThrowsArgumentException()
        {
            // Arrange
            var participants = new SortedDictionary<string, List<MusicalInstrument>>();

            // Act
            participants.GetMinStringCount();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetMinStringCount_NoGuitars_ThrowsInvalidOperationException()
        {
            // Arrange
            var participants = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["Jora"] = new List<MusicalInstrument> { new Piano("Grand", 100, "Octave", 88) },
                ["Grisha"] = new List<MusicalInstrument> { new MusicalInstrument("UraNetToi", 101) }
            };

            // Act
            participants.GetMinStringCount();
        }

        [TestMethod]
        public void GetMinStringCount_HasGuitars_ReturnsCorrectMin()
        {
            // Arrange
            var participants = new SortedDictionary<string, List<MusicalInstrument>>
            {
                ["Jora"] = new List<MusicalInstrument>
        {
            new Guitar("Stratocaster", 100, 6),
            new Guitar("Les Paul", 101, 7)
        },
                ["Grisha"] = new List<MusicalInstrument>
        {
            new Guitar("Bass", 102, 4)
        },
                ["Sergey"] = new List<MusicalInstrument>
        {
            new Guitar("Classic", 103, 5)
        }
            };

            // Act
            int result = participants.GetMinStringCount();

            // Assert
            Assert.AreEqual(4, result); // Bass имеет 4 струны
        }
    }
}
