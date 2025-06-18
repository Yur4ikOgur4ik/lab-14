using MusicalInstruments;
using Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);
    public class MyObservableCollection<T> : MyCollection<T>
        where T: IInit, ICloneable, new()
    {
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;
        public string Name { get; set; }
        
        protected virtual void CountChanged(string name, string changeType, T changedItem)
        {
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(name, changeType, changedItem));
        }

        protected virtual void OnChanged(string name, string changeType, T changedItem)
        {
            CollectionReferenceChanged.Invoke(this, new CollectionHandlerEventArgs(name, changeType, changedItem));
        }

        public override void Add(T item)
        {
            base.Add(item);
            CountChanged(this.Name, "Добавлен элемент", item);
        }

        public override bool Remove(T item)
        {
            bool result = base.Remove(item);
            if (result)
                CountChanged(this.Name, "Удалён элемент", item);
            return result;
        }

        public T this[T key]
        {
            get
            {
                if (!this.Contains(key))
                    throw new KeyNotFoundException("Объект не найден в коллекции");

                foreach (var item in this)
                {
                    if (item.Equals(key))
                        return item;
                }

                throw new InvalidOperationException("Элемент не найден");
            }
            set
            {
                if (!this.Remove(key))
                    throw new KeyNotFoundException("Не удалось найти элемент для замены");

                this.Add(value);
                OnChanged(this.Name, "Элемент заменён", value);
            }
        }
        //public string PrintTable()
        //{
        //    return base.PrintHS(); 
        //}

    }
}
