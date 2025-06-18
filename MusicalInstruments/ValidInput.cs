using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalInstruments
{
    public static class ValidInput
    {
        public static int GetInt(string message = "Input an integer")
        {
            int result;

            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out result))
                    return result;
                else
                    Console.WriteLine("Error: the number is not integer, enter again");
            }
        }

        public static int GetPositiveInt(string message = "Input an integer > 0")
        {
            int result;

            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result>0)
                    return result;
                else
                    Console.WriteLine("Error: the number is not integer > 0, enter again");
            }
        }

        public static double GetDouble(string message = "Input a real number")
        {
            double result;

            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if (double.TryParse(input, out result))
                    return result;
                else
                    Console.WriteLine("Error: the number is not real, enter again");
            }

        }

        public static MusicalInstrument CreateRandomInstr()
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
    }
}
