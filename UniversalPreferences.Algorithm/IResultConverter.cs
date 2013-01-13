using System.Collections.Generic;

namespace UniversalPreferences.Algorithm
{
    public interface IResultConverter
    {
        string Convert(IEnumerable<ushort> preferences);
    }
}