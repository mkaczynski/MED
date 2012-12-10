using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class ModifiedApriori : IAlgorithm
    {
        private readonly ICandidatesGenerator candidatesGenerator;

        private Dictionary<string, SimpleRow> temporaryResults;
        private IList<ushort[]> results;

        public ModifiedApriori(ICandidatesGenerator candidatesGenerator)
        {
            this.candidatesGenerator = candidatesGenerator;
        }

        public IEnumerable<ushort[]> FindPreferences(IEnumerable<Row> transactions)
        {
            results = new List<ushort[]>();

            var itemsets = candidatesGenerator.FindSetsWhichHasOneElement(transactions);
            itemsets = PruneResults(itemsets, transactions);

            for (int i = 1; i < transactions.First().Attributes.Length; ++i)
            {
                itemsets = candidatesGenerator.GetCandidates(itemsets, transactions);
                itemsets = PruneResults(itemsets, transactions);
            }

            return results;
        }

        private IEnumerable<ushort[]> PruneResults(IEnumerable<ushort[]> itemsets, IEnumerable<Row> transactions)
        {
            temporaryResults = new Dictionary<string, SimpleRow>();

            CheckItemsets(itemsets, transactions);

            var res = SelectPreferencesAndGetCandidates();
            temporaryResults = null;
            return res;
        }

        private void CheckItemsets(IEnumerable<ushort[]> itemsets, IEnumerable<Row> transactions) //todo: lepsza nazwa
        {
            //tutaj chyba mozna wykorzystac drzewo
            foreach (var itemset in itemsets)
            {
                var test = true;
                foreach (var transaction in transactions)
                {
                    if (Helper.IsItemsetSupported(itemset, transaction))
                    {
                        var description = GetDescription(itemset);
                        AddNode(description, itemset);
                        IncrementCounters(description, transaction);
                        test = false;
                    }
                }

                if(test)
                {
                    results.Add(itemset);
                }
            }
        }

        private IEnumerable<ushort[]> SelectPreferencesAndGetCandidates()
        {
            var tmp = new List<ushort[]>();

            foreach (var row in temporaryResults.Values)
            {
                if(row.RelationNotComplied == 0) 
                    // jezeli kandydat nie wystepuje w tej klasie, to jest minimalnym poszukiwanym wzorcem
                {
                    results.Add(row.Transaction);
                }

                if(row.RelationComplied != 0)
                    //te ktore maja 0 odrzucamy
                {
                    tmp.Add(row.Transaction);
                }
            }

            return tmp;
        }

        private void AddNode(string description, ushort[] itemset)
        {
            if (!temporaryResults.ContainsKey(description))
            {
                temporaryResults.Add(description, new SimpleRow(itemset));
            }
        }

        private void IncrementCounters(string description, Row transaction)
        {
            if (transaction.Value == Relation.Complied)
            {
                temporaryResults[description].RelationComplied += 1;
            }
            else
            {
                temporaryResults[description].RelationNotComplied += 1;
            }
        }

        private string GetDescription(IEnumerable<ushort> table)
        {
            var sb = new StringBuilder();
            foreach(var elem in table)
            {
                sb.Append(elem);
            }
            return sb.ToString();
        }

        //W celu zapewnienia 
        //odpowiedniej efektywności procedury obliczania wsparcia zbiorów kandydujących, algorytm 
        //Apriori wykorzystuje strukturę danych postaci drzewa haszowego, która służy do przechowywania 
        //zbiorów kandydujących. Procedura subset() zwraca te zbiory kandydujące należące do C_k, które 
        //są wspierane przez transakcję t
    }
}