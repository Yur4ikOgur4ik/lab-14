using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public class Piano : MusicalInstrument, IInit
    {
        private string keyLayout;
        private int keyCount;

        public string KeyLayout
        {
            get {return keyLayout;}
            set
            {
                string[] validLayouts = { "Octave", "Scale", "Digital", "Acoustic", "C", "C-Sharp (D-Flat)", "Minor", "Major", "D", "D-Sharp (E-Flat)", "E", "F", "F-Sharp (G-Flat)" };
                if (!Array.Exists(validLayouts, s => s.Equals(value, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException("Invalid keyboard layout.");
                keyLayout = value;
            }
        }

        public int KeyCount
        {
            get { return keyCount; }
            set 
            {
                if (value < 25 || value > 104)
                    throw new ArgumentException("Number of keys must be between 25 and 104");
                keyCount = value;
            }
        }

        public Piano() : base()
        {
            KeyLayout = "Octave";
            KeyCount = 88;
        }

        public Piano(string name, int id, string keyLayout,  int keyCount) : base(name, id)
        {
            KeyLayout = keyLayout;
            KeyCount = keyCount;
        }

        public override void ShowVirtual()
        {
            base.ShowVirtual();
            Console.WriteLine($"Number of keys {KeyCount}");
            Console.WriteLine($"Key layout: {KeyLayout}");
        }
        public void Show()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Key layout: {KeyLayout}");
            Console.WriteLine($"Number of keys: {KeyCount}");
        }


        public override string ToString() 
        {
            return $"{base.ToString()}, key layout: {KeyLayout}, number of keys: {KeyCount}";
        }

        public override void Init()
        {
            base.Init();
            bool isValid = false;
            while (!isValid)
            {
                try
                {
                    Console.WriteLine("Enter key layout");
                    KeyLayout = Console.ReadLine().Trim();
                    isValid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Try again.");
                }
            }
            isValid = false;
            while (!isValid)
            {
                try
                {
                    Console.WriteLine("Enter key count");
                    KeyCount = ValidInput.GetInt();
                    isValid = true;
                }
                catch (Exception ex) when (ex is ArgumentException)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Try again.");
                }
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] layouts = { "Octave", "Scale", "Digital", "Acoustic", "C", "C-Sharp (D-Flat)", "Minor", "Major", "D", "D-Sharp (E-Flat)", "E", "F", "F-Sharp (G-Flat)" };
            KeyLayout = layouts[rnd.Next(layouts.Length)];
            KeyCount = rnd.Next(25, 105);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            Piano other = (Piano)obj;
            return string.Equals(KeyLayout, other.KeyLayout, StringComparison.OrdinalIgnoreCase) && KeyCount == other.KeyCount;
        }

        public override int GetHashCode()
        {
            return 2;
        }

        public override object Clone()
        {
            return new Piano(this.Name, this.ID.id, this.KeyLayout, this.KeyCount);
        }


        public MusicalInstrument GetBase
        {
            get
            {
                return new MusicalInstrument(this.Name, this.ID.Id); // возвращаем объект базового класса с теми же значениями
            }
        }
    }
}
