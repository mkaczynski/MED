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
        public static ushort numberOfAttributes = 6; // tak jak w pliku (wszystkie oprócz nazwy klasy)
        public SimpleData Data { get; private set; }
        public ushort[] Attributes { get; private set; }
        public ushort ClassNumber { get; private set; }

        public SimpleObject(string line, SimpleData data)
        {
            this.ObjectID = ++objectCounter;
            this.Data = data;
            this.Attributes = new ushort[numberOfAttributes];
            string[] args = line.Split(',');
            if (args.Length == numberOfAttributes+1)
            {
                this.ClassNumber = data.GetBGMappings(numberOfAttributes)[args[args.Length - 1]];
                for (int i = 0; i < numberOfAttributes; ++i)
                {
                    Attributes[i] = data.GetBGMappings(i)[args[i]];
                }
            }
        }
    }

}
