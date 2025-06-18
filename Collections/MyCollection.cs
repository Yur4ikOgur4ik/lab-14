    using MusicalInstruments;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Collections
    {
        public class MyCollection<T> : MyOpenHs<T>, ICollection<T>, IEnumerable<T>, IEnumerable 
            where T : IInit, ICloneable, new()
        {
            public MyCollection() : base() { }

            public MyCollection(int length) : base()
            {
                Random rnd = new Random();
                for (int i = 0; i < length; i++)
                {
                    T item = new T();
                    item.RandomInit();
                    Add(item);
                }
            }

            public MyCollection(MyCollection<T> c) : base()
            {
                foreach (var item in c)
                {
                    Add((T)((ICloneable)item).Clone());
                }
            }
            public int Length => base.Count;
            int ICollection<T>.Count => base.Count;
            bool ICollection<T>.IsReadOnly => false;

            void ICollection<T>.Add(T item)
            {
                base.Add(item);
            }

            void ICollection<T>.Clear()
            {
                base.Clear();
            }

            bool ICollection<T>.Contains(T item)
            {
                return  base.Contains(item);
            }

            void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array), "Array is null");

                if (arrayIndex < 0 || arrayIndex > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));

                if (array.Length - arrayIndex < Count)
                    throw new ArgumentException("Not enough space in array to copy");

                int i = arrayIndex;
                foreach (var item in this)
                {
                    if (item is ICloneable cloneable)
                    {
                    
                        array[i++] = (T)cloneable.Clone();
                    }
                    else
                    {
                        throw new Exception("Item does not support deep copy");
                    }
                }
            }

            public override bool Equals(object? obj)
            {
                return base.Equals(obj);
            }

            public IEnumerator<T> GetEnumerator()
            {
                foreach (var point in set)
                {
                    if (point != null && !point.IsDeleted)
                    {
                        yield return point.Data; //rasskazat pto drugoi Enum.
                    }
                }
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            bool ICollection<T>.Remove(T item)
            {
                return base.Remove(item);
            }

            public override string? ToString()
            {
                return base.ToString();
            }

            IEnumerator IEnumerable.GetEnumerator()//experiments with doub enum.
            {
                return GetEnumerator();
            }
            public T this[T key]
            {
                get
                {
                    int index = Math.Abs(key.GetHashCode()) % set.Length;

                    for (int i = 0; i < set.Length; i++)
                    {
                        int currentIndex = (index + i) % set.Length;
                        var current = set[currentIndex];

                        if (current == null)
                            throw new KeyNotFoundException("Element was not found in collection");

                        if (!current.IsDeleted && current.Data.Equals(key))
                            return current.Data;
                    }
                    throw new KeyNotFoundException("Element was not found in collection");
                }
                set
                {

                    if (!Remove(key))
                        throw new KeyNotFoundException("Couldn't find an element to replace");


                    Add(value);
                }
            }


        }
    }
