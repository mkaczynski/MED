using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class BaseAlgorithm
    {
        private readonly ICandidatesGenerator candidatesGenerator;

        private Dictionary<string, SimpleRow> temporaryResults;
        private IList<IEnumerable<ushort>> results;

        public BaseAlgorithm(ICandidatesGenerator candidatesGenerator)
        {
            this.candidatesGenerator = candidatesGenerator;
        }

        public IEnumerable<IEnumerable<ushort>> FindPreferences(IEnumerable<Row> transactions)
        {
            Initialize(transactions);
            results = new List<IEnumerable<ushort>>();

            var itemsets = candidatesGenerator.FindSetsWhichHasOneElement(transactions);
            itemsets = PruneResults(itemsets, transactions);

            return results;

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

        protected virtual void Initialize(IEnumerable<Row> transactions)
        {
            
        }

        protected virtual bool CheckIfAnySubsetIsGenerator(SimpleRow row)
        {                
            return false;
        }

        private IEnumerable<IEnumerable<ushort>> PruneResults(IEnumerable<IEnumerable<ushort>> itemsets, IEnumerable<Row> transactions)
        {
            temporaryResults = new Dictionary<string, SimpleRow>();

            CheckItemsets(itemsets, transactions);

            var res = SelectPreferencesAndGetCandidates();
            temporaryResults = null;
            return res;
        }

        private void CheckItemsets(IEnumerable<IEnumerable<ushort>> itemsets, IEnumerable<Row> transactions) //todo: lepsza nazwa
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var copy = CreateItemsetsCopy(itemsets);
            var hashTree = CreateTree(itemsets);
            
            foreach (var transaction in transactions)
            {
                var supported = hashTree.GetSupportedSets(transaction);

                foreach (var simpleRow in supported)
                {
                    var description = GetDescription(simpleRow.Attributes);
                    var row = AddNode(description, new SimpleRow(simpleRow.Attributes));
                    IncrementCounters(row, transaction);
                    Remove(copy, description);
                }
            }

            foreach (var notSupported in copy.Values)
            {
                var description = GetDescription(notSupported);
                AddNode(description, new SimpleRow(notSupported));
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        private IDictionary<string, IEnumerable<ushort>> CreateItemsetsCopy(IEnumerable<IEnumerable<ushort>> itemsets)
        {
            var dict = new Dictionary<string, IEnumerable<ushort>>();
            foreach (var itemset in itemsets)
            {
                var description = GetDescription(itemset);
                dict.Add(description, itemset);
            }
            return dict;
        }

        private IHashTree CreateTree(IEnumerable<IEnumerable<ushort>> itemsets)
        {
            var tree = HashTreeFactory.Create(itemsets.First().Count(), 100, 2999);
            tree.FillTree(itemsets.Select(x => new Row { Attributes = x.ToArray() }));

            return tree;
        }

        private void Remove(IDictionary<string, IEnumerable<ushort>> copy, string description)
        {
            if (copy.Count > 0)
            {
                copy.Remove(description);   
            }
        }

        private IEnumerable<IEnumerable<ushort>> SelectPreferencesAndGetCandidates()
        {
            var tmp = new List<IEnumerable<ushort>>();

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
                    if (CheckIfAnySubsetIsGenerator(row))
                    {
                        rejected.Add(row);
                    }
                    else
                    {
                        tmp.Add(row.Transaction);
                        toAnalyze.Add(row);                        
                    }
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

        private SimpleRow AddNode(string description, SimpleRow row)
        {
            SimpleRow outRow;
            if(!temporaryResults.TryGetValue(description, out outRow))
            {
                outRow = row;
                temporaryResults.Add(description, row);
                OnAddNode(description, row);
            }
            return outRow;
        }

        protected virtual void OnAddNode(string description, SimpleRow row)
        {

        }

        private void IncrementCounters(SimpleRow row, Row transaction)
        {
            if (transaction.Value == Relation.Complied)
            {
                row.RelationComplied += 1;
            }
            else
            {
                row.RelationNotComplied += 1;
            }
        }

        protected string GetDescription(IEnumerable<ushort> table)
        {
            var sb = new StringBuilder();
            foreach(var elem in table)
            {
                sb.Append(elem);
            }
            return sb.ToString();
        }
    }
}