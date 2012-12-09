using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class ModifiedApriori : IAlgorithm
    {
        private Dictionary<string, SimpleRow> temporaryResults;
        private IList<ushort[]> results;

        public IEnumerable<ushort[]> FindPreferences(IEnumerable<Row> transactions)
        {
            results = new List<ushort[]>();

            var itemsets = FindSetsWhichHasOneElement(transactions);
            itemsets = PruneResults(itemsets, transactions);

            for (int i = 1; i < transactions.First().Attributes.Length; ++i)
            {
                itemsets = GetCandidates(itemsets, transactions, i+1);
                itemsets = PruneResults(itemsets, transactions);
            }

            return results;
        }

        private IEnumerable<ushort[]> 
            FindSetsWhichHasOneElement(IEnumerable<Row> transactions)
        {
            var dict = new Dictionary<ushort, ushort>(); 

            foreach(var transaction in transactions)
            {
                foreach(var value in transaction.Attributes)
                {
                    if(value.HasValue && !dict.ContainsKey(value.Value))
                    {
                        dict[value.Value] = value.Value;
                    }
                }
            }

            var res = dict.Select(x => new ushort[] {x.Key});
            return res;
        }

        private IEnumerable<ushort[]> PruneResults(IEnumerable<ushort[]> itemsets, IEnumerable<Row> transactions)
        {
            temporaryResults = new Dictionary<string, SimpleRow>();

            CheckItemsets(itemsets, transactions);

            var res = SelectPreferencesAndGetCandidates();
            temporaryResults = null;
            return res;
        }

        private IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<Row> transactions, int len)
        {
            var newCandidates = new List<ushort[]>();

            //tutaj chyba mozna wykorzystac drzewo
            foreach(var transaction in transactions)
            {
                foreach(var candidate in previousCandidates)
                {
                    if(IsItemsetSupported(candidate, transaction))
                    {
                        var candidates = GenerateCandidates(candidate, transaction);
                        foreach(var c in candidates)
                        {
                            if(!newCandidates.Any(x => x.SequenceEqual(c)))
                            {
                                newCandidates.Add(c);                                
                            }
                        }
                    }
                }
            }

            return newCandidates;
        }

        //todo: zoptymalizowac/zrefaktoryzowac
        private IEnumerable<ushort[]> GenerateCandidates(ushort[] candidate, Row transaction)
        {
            var res = new List<ushort[]>();

            for(int i = 0; i < transaction.Attributes.Length; ++i)
            {
                if(!transaction.Attributes[i].HasValue)
                {
                    continue;
                }

                var @continue = false;
                for(int j = 0; j < candidate.Length; ++j)
                {
                    if (candidate[j] == transaction.Attributes[i].Value)
                    {
                        @continue = true;
                        continue;
                    }
                }

                if(@continue)
                {
                    continue;
                }

                var newCandidate = new ushort[candidate.Length + 1];
                for(int j = 0; j < candidate.Length; ++j)
                {
                    newCandidate[j] = candidate[j];
                }
                newCandidate[candidate.Length] = transaction.Attributes[i].Value;
                Array.Sort(newCandidate);
                res.Add(newCandidate);
            }

            return res;
        }

        private void CheckItemsets(IEnumerable<ushort[]> itemsets, IEnumerable<Row> transactions) //todo: lepsza nazwa
        {
            //tutaj chyba mozna wykorzystac drzewo
            foreach (var itemset in itemsets)
            {
                foreach (var transaction in transactions)
                {
                    if (IsItemsetSupported(itemset, transaction))
                    {
                        var description = GetDescription(itemset);
                        AddNode(description, itemset);
                        IncrementCounters(description, transaction);
                    }
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

        private bool IsItemsetSupported(ushort[] itemset, Row transaction)
        {
            var contains = true;
            for (int i = 0; i < itemset.Length; ++i)
            {
                if (!transaction.Attributes.Contains(itemset[i]))
                {
                    contains = false;
                }
            }
            return contains;
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