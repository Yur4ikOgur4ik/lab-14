using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public class ElectroGuitar : Guitar, IInit, IFormattable
    {
        private string powerSource;

        public string PowerSource
        {
            get { return powerSource; }
            set
            {
                string[] validSources = { "Batteries", "Battery", "Fixed Power", "USB" };
                bool isValid = false;

                //cheching if value in valid array
                foreach (string source in validSources)
                {
                    if (source.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        isValid = true;
                        break; 
                    }
                }

                //if value not in array
                if (!isValid)
                {
                    throw new ArgumentException("Invalid power source.");
                }

                // if in
                powerSource = value;
            }


        }

        public ElectroGuitar() : base()
        {
            PowerSource = "Battery";
        }

        public ElectroGuitar(string name, int id, int stringCount, string powerSource) : base(name, id, stringCount)
        {
            PowerSource = powerSource;
        }

        //public override void ShowVirtual()
        //{
        //    base.ShowVirtual();
        //    Console.WriteLine($"Power source: {PowerSource}");
        //}
        //public void Show()
        //{
        //    Console.WriteLine($"Instrument is: {Name}");
        //    Console.WriteLine($"Number of strings = {StringCount}");
        //    Console.WriteLine($"Power source: {PowerSource}");
        //}

        //public override string ToString()
        //{
        //    return $"{base.ToString()}, power source: {PowerSource}";
        //}

        public override void Init()
        {
            base.Init();

            bool isValid = false;
            while (!isValid)
            {
                try
                {
                    PowerSource = Console.ReadLine().Trim();
                    isValid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Try again.");
                }
            }
        }

        public override void RandomInit()
        {
            base.RandomInit();
            string[] randomSource = { "Batteries", "Battery", "Fixed Power", "USB" };
            PowerSource = randomSource[rnd.Next(randomSource.Length)];
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            ElectroGuitar other = (ElectroGuitar)obj;
            
            return base.Equals(obj) && string.Equals(PowerSource, other.PowerSource, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ PowerSource.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";//.Tostring obicni
            switch (format) 
            {
                case "G":
                    return $"{base.ToString()}, power source: {PowerSource}";
                    
                case "P&S":
                    return $"Power source {PowerSource}, number of strings {StringCount}";
                default:
                    throw new Exception("wrong format");
            }
        }

        public override object Clone()
        {
            return new ElectroGuitar(this.Name, this.ID.id, this.StringCount, this.PowerSource);
        }
    }
}
