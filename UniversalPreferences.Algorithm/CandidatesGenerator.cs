using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class CandidatesGenerator : ICandidatesGenerator
    {
        private readonly int hashTreePageSize;
        private readonly int hashTreeKey;
        private Func<SimpleRow, int> calculateMin;

        public CandidatesGenerator(int hashTreePageSize, int hashTreeKey)
        {
            this.hashTreePageSize = hashTreePageSize;
            this.hashTreeKey = hashTreeKey;
        }

        public void Initialize(Func<SimpleRow, int> func)
        {
            calculateMin = func;
        }

        public IList<SimpleRow>
            FindSetsWhichHasOneElement(IEnumerable<Row> transactions)
        {
            var dict = new Dictionary<ushort, ushort>();

            var compiled = transactions.Count(x => x.Value == Relation.Complied);
            var notCompiled = transactions.Count() - compiled;

            foreach (var transaction in transactions)
            {
                foreach (var value in transaction.Attributes)
                {
                    if (!dict.ContainsKey(value))
                    {
                        dict[value] = value;
                    }
                }
            }

            var res = dict.Select(x => new[] { x.Key });
            res = res.OrderBy(x => x[0]);
            
            return res.Select(x => new SimpleRow(x) { MinRelationComplied = compiled, MinRelationNotComplied = notCompiled }).ToList();
        }

        public IList<SimpleRow> GetCandidates(IList<SimpleRow> previousCandidates, IList<SimpleRow> results, IEnumerable<Row> transactions)
        {
            var L = previousCandidates.First().Transaction.Count();
            var hashTree = HashTreeFactory.CreateCandidateTree(L, hashTreePageSize, hashTreeKey);
            hashTree.FillTree(previousCandidates);

            var newCandidates = new List<SimpleRow>();
            
            for (int i = 0; i < previousCandidates.Count; i++)
            {
                ushort[] first = previousCandidates[i].Transaction;

                for (int j = i + 1; j < previousCandidates.Count; j++)
                {
                    var second = previousCandidates[j].Transaction;
                    
                    if(!AreEqual(first, second, L-1))
                    {
                        break;
                    }
                    var newCand = new ushort[L+1];
                    for (int k = 0; k < L-1; k++)
                    {
                        newCand[k] = first[k];
                    }
                    var min = Math.Min(first[L-1], second[L-1]);
                    var max = Math.Max(first[L-1], second[L-1]);
                    newCand[L - 1] = min;
                    newCand[L] = max;

                    var sr = new SimpleRow(newCand);
                    var supportedPrevCandidates = hashTree.GetSupportedSets(sr);
                    if (supportedPrevCandidates.Count() == L + 1 &&
                            !results.Any(x => !x.Transaction.Except(newCand).Any()))
                    {
                        CompleteCounters(sr, supportedPrevCandidates);

                        newCandidates.Add(sr);
                    }
                    
                }
            }

            return newCandidates;
        }

        private void CompleteCounters(SimpleRow simpleRow, IEnumerable<SimpleRow> previous)
        {
            if(calculateMin == null)
                return;
           
            var min = previous.Min(x => calculateMin(x));
            var elem = previous.FirstOrDefault(x => calculateMin(x) == min);

            simpleRow.MinRelationComplied = elem.RelationComplied;
            simpleRow.MinRelationNotComplied = elem.RelationNotComplied;
        }

        private bool AreEqual(ushort[] x, ushort[] y, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if(x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}