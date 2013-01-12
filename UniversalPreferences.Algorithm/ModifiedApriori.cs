using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

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

            while (true)
            {
                itemsets = candidatesGenerator.GetCandidates(itemsets, results, transactions);
                if (!itemsets.Any())
                {
                    break;
                }
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
            var copy = new List<ushort[]>(itemsets);

            var hashTree = HashTreeFactory.Create(itemsets.First().Length, 2, 3);
            hashTree.FillTree(itemsets.Select(x => new Row { Attributes = x }));
            
            foreach (var transaction in transactions)
            {
                var supported = hashTree.GetSupportedSets(transaction);

                foreach (var simpleRow in supported)
                {
                    var description = GetDescription(simpleRow.Attributes);
                    AddNode(description, simpleRow.Attributes);
                    IncrementCounters(description, transaction);
             
                    copy.Remove(copy.FirstOrDefault(x => x.SequenceEqual(simpleRow.Attributes)));
                }
            }

            foreach (var notSupported in copy)
            {
                var description = GetDescription(notSupported);
                AddNode(description, notSupported);
            }
        }

        private IEnumerable<ushort[]> SelectPreferencesAndGetCandidates()
        {
            var tmp = new List<ushort[]>();

            var found = new List<SimpleRow>();
            var toAnalyze = new List<SimpleRow>();
            var rejected = new List<SimpleRow>();

            foreach (var row in temporaryResults.Values)
            {
                if(row.RelationNotComplied == 0) 
                    // jezeli kandydat nie wystepuje w tej klasie, to jest minimalnym poszukiwanym wzorcem
                {
                    results.Add(row.Transaction);
                    found.Add(row);
                }
                else if(row.RelationComplied != 0) //te ktore maja 0 odrzucamy
                {
                    tmp.Add(row.Transaction);
                    toAnalyze.Add(row);
                }
                else //odrzucone
                {
                    rejected.Add(row);
                }
            }

#if DEBUG
            WriteInfo(found, toAnalyze, rejected);
#endif
            return tmp;
        }

        private void WriteInfo(IEnumerable<SimpleRow> found, IEnumerable<SimpleRow> toAnalyze, IEnumerable<SimpleRow> rejected)
        {
            System.Diagnostics.Debug.WriteLine("\nIteracja");
            System.Diagnostics.Debug.WriteLine("Znalezione");
            WriteListInfo(found);
            System.Diagnostics.Debug.WriteLine("Do analizy");
            WriteListInfo(toAnalyze);
            System.Diagnostics.Debug.WriteLine("Odrzucone");
            WriteListInfo(rejected);
        }

        private void WriteListInfo(IEnumerable<SimpleRow> list)
        {
            foreach (SimpleRow simpleRow in list)
            {
                System.Diagnostics.Debug.WriteLine(simpleRow);
            }
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