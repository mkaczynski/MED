using System.Collections.Generic;

namespace UniversalPreferences.Algorithm
{
    public class ResultConverter : IResultConverter
    {
        //mapownia w formie <indeks w tablicy, nazwa atrybutu>
        private readonly Dictionary<int, string> mappings;

        public ResultConverter(Dictionary<int, string> mappings)
        {
            this.mappings = mappings;
        }

        public string Convert(bool[] preferences)
        {
            //todo
            return null;
        }
    }
}