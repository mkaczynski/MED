using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class BaseAlgorithm
    {
        int found = 0, toAnalyze = 0, rejected = 0;

        private readonly bool writeIterationResultsToFile;
        private bool generators;
        private Func<SimpleRow, bool> hasGenerator; 

        private void InitializeGeneratorMethod(string method)
        {
            if(method == "P")
                return;

            generators = true;
            if (method == "G")
                hasGenerator =
                    row => row.MinRelationComplied + row.MinRelationNotComplied ==
                           row.RelationComplied + row.RelationNotComplied;
            else
                hasGenerator =
                    row => row.MinRelationNotComplied == row.RelationNotComplied;
        }

        public event EventHandler<DiagnosticsInfo> DiagnosticsEvent;
        private readonly ICandidatesGenerator candidatesGenerator;
        private readonly IResultConverter resultConverter;

        private IList<SimpleRow> results;

        private readonly int hashTreePageSize;
        private readonly int hashTreeKey;

        public BaseAlgorithm(int hashTreePageSize, int hashTreeKey, bool writeIterationResultsToFile, string method, 
            ICandidatesGenerator candidatesGenerator, IResultConverter resultConverter)
        {
            this.hashTreePageSize = hashTreePageSize;
            this.hashTreeKey = hashTreeKey;
            this.resultConverter = resultConverter;

            this.candidatesGenerator = candidatesGenerator;

            this.writeIterationResultsToFile = writeIterationResultsToFile;
            
            InitializeGeneratorMethod(method);
        }

        public IEnumerable<SimpleRow> FindPreferences(IEnumerable<Row> transactions)
        {
            Initialize(transactions);
            results = new List<SimpleRow>();

            Console.WriteLine("Informacje dla kandydatow o dl. 1\n");

            var itemsets = candidatesGenerator.FindSetsWhichHasOneElement(transactions);
            itemsets = PruneResults(itemsets, transactions);

            WriteInfo(found, toAnalyze, rejected);

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

                Console.WriteLine("Informacje dla kandydatow o dl. {0}\n", tranLength);
                itemsets = PruneResults(itemsets, transactions);
                watch.Stop();
                Console.WriteLine("Czas obrotu petli dla transakcji o dlugosci {0}: {1}", tranLength, watch.Elapsed);

                WriteInfo(found, toAnalyze, rejected);
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

        private IList<SimpleRow> PruneResults(IList<SimpleRow> itemsets, IEnumerable<Row> transactions)
        {
            var hashTree = CheckItemsets(itemsets, transactions);
            var res = SelectPreferencesAndGetCandidates(hashTree);
            return res;
        }

        private IHashTree CheckItemsets(IEnumerable<SimpleRow> itemsets, IEnumerable<Row> transactions) //todo: lepsza nazwa
        {
            var sw = new Stopwatch();
            sw.Start();

            var hashTree = CreateTree(itemsets);
            
            foreach (var transaction in transactions)
            {
                var supported = hashTree.GetSupportedSets(transaction);
                foreach (var simpleRow in supported)
                {
                    IncrementCounters(simpleRow, transaction);
                }
            }

            sw.Stop();
            Console.WriteLine("Czas obliczania wsparc znalezionych kandydatow: " + sw.Elapsed);

            return hashTree;
        }

        private IHashTree CreateTree(IEnumerable<SimpleRow> itemsets)
        {
            var tree = HashTreeFactory.Create(itemsets.First().Transaction.Length, hashTreePageSize, hashTreeKey);
            tree.FillTree(itemsets);

            return tree;
        }

        private IList<SimpleRow> SelectPreferencesAndGetCandidates(IHashTree hashTree)
        {
            var tmp = new List<SimpleRow>();
            var curResults = new List<SimpleRow>();

            found = 0;
            toAnalyze = 0; 
            rejected = 0;

            foreach (var row in hashTree.GetRows())
            {
                if(row.RelationNotComplied == 0) 
                    // jezeli kandydat nie wystepuje w tej klasie, to jest minimalnym poszukiwanym wzorcem
                {
                    curResults.Add(row);
                    results.Add(row);
                    //OnAddNode(row);
                    found += 1;
                }
                else if(row.RelationComplied != 0) //te ktore maja 0 odrzucamy
                {
                    if ( 
                        /*row.MinRelationComplied +*/ row.MinRelationNotComplied == /*row.RelationComplied +*/ row.RelationNotComplied)
                    {
                        rejected += 1;
                    }
                    else
                    {
                        tmp.Add(row);
                        toAnalyze += 1;                        
                    }
                }
                else //odrzucone
                {
                    rejected += 1;
                }
            }

            WriteResultsToFile(curResults);

            return tmp;
        }

        private void WriteResultsToFile(List<SimpleRow> curResults)
        {
            var filePath = "wyniki_z_iteracji.txt";
            if (writeIterationResultsToFile)
            {
                using (var streamWriter = new StreamWriter(filePath, true))
                {
                    curResults.ForEach(x => 
                        streamWriter.WriteLine(string.Format("{0}", resultConverter.Convert(x.Transaction))));
                }
            }
        }

        private void WriteInfo(int found, int toAnalyze, int rejected)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Znalezione: " + found);
            //WriteListInfo(sb, found);
            sb.AppendLine("Do analizy: " + toAnalyze);
            //WriteListInfo(sb, toAnalyze);
            sb.AppendLine("Odrzucone:  " + rejected);
            //WriteListInfo(sb, rejected);
            sb.AppendLine();
            sb.AppendLine("========================");
            sb.AppendLine();

            OnDiagnosticsEvent(new DiagnosticsInfo(sb.ToString()));
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