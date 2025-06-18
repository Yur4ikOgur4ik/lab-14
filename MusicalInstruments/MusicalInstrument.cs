
using System.Reflection.Metadata.Ecma335;

namespace MusicalInstruments
{
    public class MusicalInstrument : IInit, IComparable<MusicalInstrument>, ICloneable
    {
        protected string name;
        protected Random rnd;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))//added orWhiteSpace for "   "
                    throw new ArgumentNullException("Name cannot be empty");//chech for probeli
                name = value;
            }
        }

        public IdNumber ID { get;set; }


        public MusicalInstrument()//bez parametrov
        {
            Name = "Unknown Instrument";
            ID = new IdNumber(0);
            rnd = new Random();
        }

        public MusicalInstrument(string name, int id)//s parametrom
        {
            Name = name;
            ID = new IdNumber(id);
            rnd = new Random();
        }

        public virtual void ShowVirtual()
        {
            Console.WriteLine($"Instrument name: {Name}");
        }
        public void Show()
        {
            Console.WriteLine($"Instrument is: {Name}");
        }

        public override string ToString()
        {
            return $"Id: {ID}, name - {Name}";
        }

        public virtual void Init()//Vvod nazvaniya instrumenta
        {
            bool isValid = false;
            while (!isValid)
            {
                try
                {
                    Console.WriteLine("Enter Id");
                    ID.Id = ValidInput.GetInt();
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
                    Console.WriteLine("Enter name");
                    Name = Console.ReadLine().Trim();
                    isValid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Try again.");
                }
            }
        }

        public virtual void RandomInit()
        {
            string[] names = {"Garmoshka", "Antowka", "CoolInstrument", "Goida", "RandomName123", "WhoCares", "UraNetToi"};
            ID.id = rnd.Next(1, 100);
            Name = names[rnd.Next(names.Length)];
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())//chech if null or different types
                return false; 

            MusicalInstrument other = (MusicalInstrument)obj;
            return (string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) && ID.Equals(other.ID));//compare ignoring register (A = a)
        }

        public override int GetHashCode()//pereopredelyaem hash code
        {
            return Name.GetHashCode()^ID.id.GetHashCode();
        }



        public int CompareTo(MusicalInstrument? other)
        {
            if (other == null)
                return -1;
            return Name.CompareTo(other.Name);
        }

        public virtual object Clone()
        {
            return new MusicalInstrument(this.Name, this.ID.Id);
        }

        public object ShallowCopy()
        {
            return this.MemberwiseClone();
        }

        
    }
}
