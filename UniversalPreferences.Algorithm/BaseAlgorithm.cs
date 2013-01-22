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
        public event EventHandler<DiagnosticsInfo> DiagnosticsEvent;
        private readonly ICandidatesGenerator candidatesGenerator;

        private IList<ushort[]> results;

        private readonly int hashTreePageSize;
        private readonly int hashTreeKey;

        public BaseAlgorithm(int hashTreePageSize, int hashTreeKey, ICandidatesGenerator candidatesGenerator)
        {
            this.hashTreePageSize = hashTreePageSize;
            this.hashTreeKey = hashTreeKey;

            this.candidatesGenerator = candidatesGenerator;
        }

        public IEnumerable<IEnumerable<ushort>> FindPreferences(IEnumerable<Row> transactions)
        {
            Initialize(transactions);
            results = new List<ushort[]>();

            var itemsets = candidatesGenerator.FindSetsWhichHasOneElement(transactions);
            itemsets = PruneResults(itemsets, transactions);

            int tranLength = 1;
            while (true)
            {
                if (!itemsets.Any())
                {
                    break;
                }

                Stopwatch watch = Stopwatch.StartNew();
                itemsets = candidatesGenerator.GetCandidates(itemsets, results, transactions);

                tranLength++;
                if (!itemsets.Any())
                {
                    break;
                }
                itemsets = PruneResults(itemsets, transactions);
                watch.Stop();
                Console.WriteLine("Czas obrotu pętli dla transakcji o długości {0}: {1}", tranLength, watch.Elapsed);
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

        private IList<ushort[]> PruneResults(IList<ushort[]> itemsets, IEnumerable<Row> transactions)
        {
            var hashTree = CheckItemsets(itemsets, transactions);
            var res = SelectPreferencesAndGetCandidates(hashTree);
            return res;
        }

        private IHashTree CheckItemsets(IEnumerable<IEnumerable<ushort>> itemsets, IEnumerable<Row> transactions) //todo: lepsza nazwa
        {
            var sw = new Stopwatch();
            sw.Start();

            var simpleRows = itemsets.Select(x => new SimpleRow(x));
            var hashTree = CreateTree(simpleRows);
            
            foreach (var transaction in transactions)
            {
                var supported = hashTree.GetSupportedSets(transaction);
                foreach (var simpleRow in supported)
                {
                    IncrementCounters(simpleRow, transaction);
                }
            }

            sw.Stop();
            Console.WriteLine("CheckItemsets " + sw.Elapsed);

            return hashTree;
        }

        private IHashTree CreateTree(IEnumerable<SimpleRow> itemsets)
        {
            var tree = HashTreeFactory.Create(itemsets.First().Transaction.Length, hashTreePageSize, hashTreeKey);
            tree.FillTree(itemsets);

            return tree;
        }

        private IList<ushort[]> SelectPreferencesAndGetCandidates(IHashTree hashTree)
        {
            var tmp = new List<ushort[]>();

            int found = 0, toAnalyze = 0, rejected = 0;

            foreach (var row in hashTree.GetRows())
            {
                if(row.RelationNotComplied == 0) 
                    // jezeli kandydat nie wystepuje w tej klasie, to jest minimalnym poszukiwanym wzorcem
                {
                    results.Add(row.Transaction);
                    OnAddNode(row);
                    found += 1;
                }
                else if(row.RelationComplied != 0) //te ktore maja 0 odrzucamy
                {
                    if (CheckIfAnySubsetIsGenerator(row))
                    {
                        rejected += 1;
                    }
                    else
                    {
                        tmp.Add(row.Transaction);
                        toAnalyze += 1;                        
                    }
                }
                else //odrzucone
                {
                    rejected += 1;
                }
            }

            WriteInfo(found, toAnalyze, rejected);
            return tmp;
        }

        private void WriteInfo(int found, int toAnalyze, int rejected)
        {
            var sb = new StringBuilder();

            sb.AppendLine("========================");
            sb.AppendLine("Znalezione " + found);
            //WriteListInfo(sb, found);
            sb.AppendLine("Do analizy " + toAnalyze);
            //WriteListInfo(sb, toAnalyze);
            sb.AppendLine("Odrzucone" + rejected);
            //WriteListInfo(sb, rejected);
            sb.AppendLine();

            OnDiagnosticsEvent(new DiagnosticsInfo(sb.ToString()));
        }

        private void WriteListInfo(StringBuilder sb, IEnumerable<SimpleRow> list)
        {
            foreach (SimpleRow simpleRow in list)
            {
                sb.AppendLine(simpleRow.ToString());
            }
        }

        protected virtual void OnAddNode(SimpleRow row)
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
                sb.Append(",");
            }
            return sb.ToString();
        }

        protected virtual void OnDiagnosticsEvent(DiagnosticsInfo e)
        {
            EventHandler<DiagnosticsInfo> handler = DiagnosticsEvent;
            if (handler != null) handler(this, e);
        }
    }
}