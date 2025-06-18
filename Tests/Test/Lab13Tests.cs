using Arguments;
using Collections;
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
        [ExpectedException(typeof(InvalidOperationException))]
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

    }
}
