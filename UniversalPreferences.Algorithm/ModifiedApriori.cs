using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class ModifiedApriori : IAlgorithm
    {
        public IEnumerable<bool[]> FindPreferences(IEnumerable<Row> transactions)
        {
            throw new System.NotImplementedException();
        }

        //W celu zapewnienia 
        //odpowiedniej efektywności procedury obliczania wsparcia zbiorów kandydujących, algorytm 
        //Apriori wykorzystuje strukturę danych postaci drzewa haszowego, która służy do przechowywania 
        //zbiorów kandydujących. Procedura subset() zwraca te zbiory kandydujące należące do C_k, które 
        //są wspierane przez transakcję t
    }
}