using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public class Guitar : MusicalInstrument, IInit
    {
        private int stringCount;//kol-vo strun

        public int StringCount
        {
            get { return stringCount; }
            set
            {
                if (value < 3 || value > 20)
                    throw new ArgumentException("Number of strigs must be between 3 and 20");
                stringCount = value;
            }
        }

        public Guitar() : base()
        {
            StringCount = 6;
        }

        public Guitar(string name, int id, int stringCount) : base(name, id)
        {
            StringCount = stringCount;
        }

        public override void ShowVirtual()
        {
            base.ShowVirtual();
            Console.WriteLine($"Number of strings: {StringCount}");
        }
        public  void Show()
        {
            Console.WriteLine($"Instrument is: {Name}");
            Console.WriteLine($"Number of strings: {StringCount}");
        }

        public override string ToString()
        {
            return $"{base.ToString()}, number of strings: {StringCount}";
        }

        public override void Init()
        {
            base.Init();
            try
            {
                StringCount = ValidInput.GetInt();
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Because of error, making a standart number of strings (6)");
                StringCount = 6;
            }
        }

        public override void RandomInit()
        {
            ID.id = rnd.Next(0, 100);
            string[] names = { "Balalaika", "Bass guitar", "Acoustic guitar", "Classic guitar", "Guitar", "NameOfGuitar", "Gnezdo", "HeHeHeHaw"};
            Name = names[rnd.Next(names.Length)];
            StringCount = rnd.Next(3, 21);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            Guitar other = (Guitar)obj;
            return stringCount == other.StringCount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ StringCount.GetHashCode();
        }

        public override object Clone()
        {
            return new Guitar(this.Name, this.ID.id, this.StringCount);
        }
    }
}
