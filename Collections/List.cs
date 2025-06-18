using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicalInstruments;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Collections
{
    public class DoublyLinkedList<T> where T : MusicalInstrument, IInit, ICloneable
    {
        public Point<T>? begin { get; private set; }
        public Point<T>? end { get; private set; } // Добавляем указатель на конец списка

        public int Count
        {
            get
            {
                int count = 0;
                Point<T>? current = begin;
                while (current != null)
                {
                    count++;
                    current = current.Next;
                }
                return count;
            }
        }

        public DoublyLinkedList()
        {
            begin = null;
            end = null;
        }

        public void Add(T item)
        {
            Point<T> newPoint = new Point<T>(item);
            if (begin == null)
            {
                begin = newPoint;
                end = newPoint;
            }
            else
            {
                end!.Next = newPoint;
                newPoint.Prev = end;
                end = newPoint;
            }
        }



        //public void PrintList()//add for empty list 
        //{
        //    if (Count == 0)
        //        Console.WriteLine("List is empty");
        //    else
        //    {
        //        Point<T>? current = begin;
        //        int count = 1;
        //        while (current != null)
        //        {
        //            Console.WriteLine($"{count}: {current.Data}");
        //            current = current.Next;
        //            count++;
        //        }
        //    }
        //}
        public MusicalInstrument CreateRandomInstr()
        {
            Random rnd = new Random();
            MusicalInstrument instr;
            int type = rnd.Next(3);
            switch (type) 
            {
                case 0:
                    instr = new MusicalInstrument();
                    break;
                case 1:
                    instr = new Guitar();
                    break;
                case 2:
                    instr = new ElectroGuitar();
                    break;
                default:
                    instr = new Piano();
                    break;

            }
            instr.RandomInit();
            instr.Name = instr.Name + " (Rnd)";
            return instr;
        }
        // 1. Добавление элементов с номерами 1, 3, 5 и т.д.
        public void AddOddIndexElements(int count)
        {
            Point<T>? current = begin;

            //в идеальном посмтроение - достаточно перебрать элементы и переназначить ссылки, подумать над тем, чтобы сделать легче!!!!!!!!
            int addedCount = 0;
            while (current != null && addedCount < count)
            {
                // Создаём новый случайный инструмент
                T randomInstrument = (T)(object)CreateRandomInstr();

                // Добавляем новый узел после текущего узла
                AddAfter(current, randomInstrument);

                addedCount++; // Увеличиваем счётчик добавленных элементов

                // Переходим к следующему узлу (пропускаем только что добавленный)
                current = current.Next?.Next; // Пропускаем два узла: текущий и добавленный
            }

        }

        public void AddAfter(Point<T> node, T data)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Point<T> newNode = new Point<T>(data);

            newNode.Next = node.Next; // Новый узел указывает на следующий узел
            newNode.Prev = node;      // Новый узел указывает на текущий узел

            if (node.Next != null)
                node.Next.Prev = newNode; // Следующий узел указывает на новый узел

            node.Next = newNode; // Текущий узел указывает на новый узел

            if (node == end) // Если добавляем после последнего узла
                end = newNode;
        }
        // 2. Удаление всех элементов, начиная с элемента с заданным именем, и до конца списка
        public void RemoveFromElementToEnd(string name)
        {
            Point<T>? current = begin;
            bool found = false;

            while (current != null)
            {
                if (current.Data is MusicalInstrument instrument && instrument.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
                current = current.Next;
            }//ishem name if not found 

            if (found)
            {
                if (current!.Prev != null)//found ne v nachale
                {
                    current.Prev.Next = null;
                    end = current.Prev;
                }
                else// elsi v nachale to srazu clear
                {
                    begin = null;
                    end = null;
                }
            }
        }


        // 3. Глубокое клонирование списка
        public DoublyLinkedList<T> DeepClone()
        {
            DoublyLinkedList<T> newList = new DoublyLinkedList<T>();
            Point<T>? current = begin;

            while (current != null)
            {
                if (current.Data is ICloneable cloneable)
                {
                    T clonedItem = (T)cloneable.Clone();
                    newList.Add(clonedItem);
                }
                else
                {
                    // Если элемент не поддерживает клонирование, просто добавляем ссылку
                    throw new Exception("Type does not support cloning");
                }
                current = current.Next;
            }

            return newList;
        }

        // 4. Удаление списка из памяти
        public void Clear()
        {
            
            begin = null;
            end = null;
        }
    }
}

