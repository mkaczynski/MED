using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public interface IHashTree
    {
        void FillTree(IEnumerable<SimpleRow> elements); 
            // buduje drzewo na podstawie zbioru wierszy kandydujacych
            // na pewno te wiersze beda mialy po tyle samo elementow w tablicach atrybutow

        IEnumerable<SimpleRow> GetSupportedSets(SimpleRow transaction); 
            // ma zwracac te zbiory kandydujace (z nich drzewo jest zbudowane) ktore wspieraja row
    }
}