using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collections;
using MusicalInstruments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TestHC
    {
        private MyOpenHs<MusicalInstrument> hashTable;

        [TestInitialize]
        public void Setup()
        {
            // Инициализируем хэш-таблицу начальной емкостью = 4, LoadFactor = 0.72
            hashTable = new MyOpenHs<MusicalInstrument>(4, 0.72);
        }

        [TestMethod]
        public void Add_AddsNewItemSuccessfully()
        {
            var item = new Guitar("Gibson", 1, 6);
            hashTable.Add(item);

            Assert.AreEqual(1, hashTable.Count);
            Assert.IsTrue(hashTable.Contains(item));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Add_DoesNotAddNullItem_ThrowsException()
        {
            hashTable.Add(null);
        }

        [TestMethod]
        public void Add_DoesNotAllowDuplicates()
        {
            var item = new Guitar("Fender", 1, 6);
            hashTable.Add(item);
            hashTable.Add(item); // Попытка добавить дубликат

            Assert.AreEqual(1, hashTable.Count);
        }

        [TestMethod]
        public void Resize_IncreasesSizeWhenLoadFactorExceeded()
        {
            // Заполняем до предела: capacity=4, loadfactor=0.72 → max elements ≈ 2
            var item1 = new Guitar("Guitar1", 1, 6);
            var item2 = new Guitar("Guitar2", 2, 6);
            var item3 = new Guitar("Guitar3", 3, 6);
            var item4 = new Guitar("Guitar4", 4, 6);

            hashTable.Add(item1);
            hashTable.Add(item2);
            hashTable.Add(item3); 
            hashTable.Add(item4);// Вызов Resize()

            Assert.AreEqual(hashTable.Capacity > 4, true);
            Assert.AreEqual(4, hashTable.Count);
        }

        [TestMethod]
        public void Remove_RemovesExistingItemSuccessfully()
        {
            var item = new Piano("Yamaha", 1, "Octave", 88);
            hashTable.Add(item);

            bool result = hashTable.Remove(item);
            Assert.IsTrue(result);
            Assert.AreEqual(0, hashTable.Count);
            Assert.IsFalse(hashTable.Contains(item));
        }

        [TestMethod]
        public void Remove_ReturnsFalseIfItemNotFound()
        {
            var item = new Piano("Steinway", 1, "Octave", 88);
            var otherItem = new Piano("Kawai", 2, "Octave", 76);

            hashTable.Add(item);

            bool result = hashTable.Remove(otherItem);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectIndexAndData()
        {
            var item = new Guitar("Ibanez", 1, 7);
            hashTable.Add(item);

            int index;
            var found = hashTable.Find(item, out index);

            Assert.IsNotNull(found);
            Assert.AreEqual(item.Name, found.Name);
            Assert.IsTrue(index >= 0 && index < hashTable.set.Length);
        }

        [TestMethod]
        public void Contains_ReturnsTrueForExistingItems()
        {
            var item = new Guitar("ESP", 1, 6);
            hashTable.Add(item);

            Assert.IsTrue(hashTable.Contains(item));
        }

        [TestMethod]
        public void Contains_ReturnsFalseForRemovedItems()
        {
            var item = new Guitar("Jackson", 1, 6);
            hashTable.Add(item);
            hashTable.Remove(item);

            Assert.IsFalse(hashTable.Contains(item));
        }

        [TestMethod]
        public void Resize_RehashesAndInsertsCollidingElementsCorrectly()
        {
            // Arrange: начальный размер = 4
            hashTable = new MyOpenHs<MusicalInstrument>(4, 0.72);

            // Добавляем 3 элемента → вызовет Resize()
            var itemA = new Guitar("SameHash", 1, 6);
            var itemB = new Guitar("SameHash", 1, 6); // тот же GetHashCode
            var itemC = new Guitar("SameHash", 1, 6); // третий с тем же хэшем

            hashTable.Add(itemA);
            hashTable.Add(itemB);
            hashTable.Add(itemC); // должен вызвать Resize()

            // Act & Assert: проверяем, что все элементы добавлены и найдены
            Assert.AreEqual(8, hashTable.Capacity); // размер увеличился до 8
            Assert.IsTrue(hashTable.Contains(itemA));
            Assert.IsTrue(hashTable.Contains(itemB));
            Assert.IsTrue(hashTable.Contains(itemC));

            // Проверяем удаление второго элемента после первого
            hashTable.Remove(itemA);
            Assert.IsFalse(hashTable.Contains(itemA));

            Assert.IsTrue(hashTable.Contains(itemB)); // должен быть на своём месте
            Assert.IsTrue(hashTable.Contains(itemC));
        }

        [TestMethod]
        public void PrintHS_ReturnsNonEmptyString()
        {
            var item = new Guitar("PrintTest", 1, 6);
            hashTable.Add(item);

            string result = hashTable.PrintHS();
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void Clear_EmptiesTheHashTable()
        {
            var item = new Guitar("ClearTest", 1, 6);
            hashTable.Add(item);
            hashTable.Clear();

            Assert.AreEqual(0, hashTable.Count);
            Assert.IsFalse(hashTable.Contains(item));
        }
        [TestMethod]
        public void HashPoint_DefaultConstructor_SetsDefaultValues()
        {
            // Arrange & Act
            var hashPoint = new HashPoint<MusicalInstrument>();

            // Assert
            Assert.IsNull(hashPoint.Data); // Для ссылочных типов default == null
            Assert.IsFalse(hashPoint.IsDeleted);
        }
        [TestMethod]
        public void HashPoint_ToString_ReturnsCorrectValue()
        {

            // Arrange: создаём HashPoint с данными
            var Data = new Guitar("TestGuitar", 1, 6);
            var hashPointWithData = new HashPoint<MusicalInstrument>(Data);
            

            // Arrange: создаём HashPoint без данных
            var hashPointNullData = new HashPoint<MusicalInstrument>();

            // Act
            string resultWithData = hashPointWithData.ToString();
            string resultWithNull = hashPointNullData.ToString();

            // Assert
            Assert.AreEqual("TestGuitar", resultWithData); // зависит от реализации Guitar.ToString()
            Assert.AreEqual("null", resultWithNull);
        }
    }
}

