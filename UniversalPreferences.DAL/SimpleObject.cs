using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalPreferences.DAL
{
    public class SimpleObject
    {
        public static ushort objectCounter = 0;
        public ushort ObjectID { get; private set; }
        //public static ushort numberOfAttributes = 6; // tak jak w pliku (wszystkie oprócz nazwy klasy)
        public DataFromFile Data { get; private set; }
        public ushort[] Attributes { get; private set; }
        public ushort ClassNumber { get; private set; }

        public SimpleObject(string line, DataFromFile data)
        {
            this.ObjectID = ++objectCounter;
            this.Data = data;
            this.Attributes = new ushort[Data.numberOfAttributes];
            string[] args = line.Split(',');
            if (args.Length == Data.numberOfAttributes + 1)
            {
                this.ClassNumber = Data.GetBGMappings(Data.numberOfAttributes)[args[args.Length - 1]];
                for (int i = 0; i < Data.numberOfAttributes; ++i)
                {
                    Attributes[i] = Data.GetBGMappings(i)[args[i]];
                }
            }
        }
    }

}
