using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collections;
using MusicalInstruments;

namespace Test
{
    [TestClass]
    public class DoublyLinkedListTests
    {

        [TestMethod]
        public void Point_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var point = new Point<string>();

            // Assert
            Assert.IsNull(point.Data);         // Data должен быть default(T), т.е. null для string
            Assert.IsNull(point.Next);         // Next должен быть null
            Assert.IsNull(point.Prev);         // Prev должен быть null
        }


        // 1. Тестирование метода Add - добавление элементов
        [TestMethod]
        public void Add_ShouldAddElementsToTheList()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };

            // Act
            list.Add(guitar);
            list.Add(piano);

            // Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Guitar", list.begin.Data.Name);
            Assert.AreEqual("Piano", list.end.Data.Name);
            Assert.IsNull(list.begin.Prev);
            Assert.IsNull(list.end.Next);
            Assert.AreSame(list.begin.Next, list.end);
            Assert.AreSame(list.end.Prev, list.begin);
        }




        // 3. Тестирование метода AddOddIndexElements - добавление на нечётные позиции
        [TestMethod]
        public void AddOddIndexElements_ShouldInsertRandomInstrumentsAtOddPositions()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var instrument1 = new Guitar { Name = "Guitar" };
            var instrument2 = new Piano { Name = "Piano" };
            list.Add(instrument1);
            list.Add(instrument2);

            // Act
            list.AddOddIndexElements(2); // Добавляем 2 случайных инструмента

            // Assert
            Assert.AreEqual(4, list.Count);

            var secondNode = list.begin.Next;
            var thirdNode = secondNode.Next;

            Assert.IsTrue(secondNode.Data.Name.Contains("(Rnd)"));
            Assert.IsTrue(thirdNode.Next.Data.Name.Contains("(Rnd)"));
        }

        // 4. Тестирование метода RemoveFromElementToEnd - удаление от найденного до конца
        [TestMethod]
        public void RemoveFromElementToEnd_ShouldRemoveElementsStartingFromGivenName()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            var electroGuitar = new ElectroGuitar { Name = "ElectroGuitar" };
            list.Add(guitar);
            list.Add(piano);
            list.Add(electroGuitar);

            // Act
            list.RemoveFromElementToEnd("Piano");

            // Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Guitar", list.begin.Data.Name);
            Assert.IsNull(list.begin.Next);
            Assert.IsNull(list.end.Next);
        }

        [TestMethod]
        public void RemoveFromElementToEnd_ShouldRemoveAllElementsWhenFirstElementIsRemoved()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            var electroGuitar = new ElectroGuitar { Name = "ElectroGuitar" };
            list.Add(guitar);
            list.Add(piano);
            list.Add(electroGuitar);

            // Act
            list.RemoveFromElementToEnd("Guitar");

            // Assert
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.begin);
            Assert.IsNull(list.end);
        }

        // 5. Тестирование метода DeepClone - глубокое копирование списка
        [TestMethod]
        public void DeepClone_ShouldCreateIndependentCopyOfList()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            list.Add(guitar);
            list.Add(piano);

            // Act
            var clonedList = list.DeepClone();

            // Assert
            Assert.AreEqual(list.Count, clonedList.Count);
            Assert.AreNotSame(list.begin.Data, clonedList.begin.Data); // разные объекты
            Assert.AreEqual(list.begin.Data.Name, clonedList.begin.Data.Name);
            Assert.AreEqual(list.end.Data.Name, clonedList.end.Data.Name);
        }

        // 6. Тестирование метода Clear - очистка списка
        [TestMethod]
        public void Clear_ShouldRemoveAllNodesAndResetPointers()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            list.Add(guitar);

            // Act
            list.Clear();

            // Assert
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.begin);
            Assert.IsNull(list.end);
        }

        // 7. Тестирование пустого списка
        [TestMethod]
        public void EmptyList_ShouldHaveZeroCountAndNullReferences()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();

            // Assert
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.begin);
            Assert.IsNull(list.end);
        }

        // 8. Тестирование метода CreateRandomInstr - генерация случайного инструмента
        [TestMethod]
        public void CreateRandomInstr_ShouldReturnValidInstrumentWithSuffix()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();

            // Act
            var instrument = list.CreateRandomInstr();

            // Assert
            Assert.IsNotNull(instrument);
            Assert.IsTrue(instrument.Name.Contains("(Rnd)"));
        }
        [TestMethod]
        public void ToStringInPoint_ReturnsValue()
        {
            var newInstr = new MusicalInstrument("Name", 1);
            var newPoint = new Point<MusicalInstrument>(newInstr);

            var toString = newPoint.ToString();

            Assert.AreEqual(toString, newInstr.ToString());
        }

        [TestMethod]
        public void ToStringInPoint_ReturnsNull() 
        { 
            var newInstr = new MusicalInstrument();
            newInstr = null;

            var newPoint = new Point<MusicalInstrument>(newInstr);
            string toString = newPoint.ToString();

            Assert.AreEqual("null", toString);
        }



    }
        
    
  }