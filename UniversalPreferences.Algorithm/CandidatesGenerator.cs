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

        public CandidatesGenerator(int hashTreePageSize, int hashTreeKey)
        {
            this.hashTreePageSize = hashTreePageSize;
            this.hashTreeKey = hashTreeKey;
        }

        public IList<ushort[]>
            FindSetsWhichHasOneElement(IEnumerable<Row> transactions)
        {
            var dict = new Dictionary<ushort, ushort>();

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
            return res.ToList();
        }

        public IList<ushort[]> GetCandidates(IList<ushort[]> previousCandidates, IList<ushort[]> results, IEnumerable<Row> transactions)
        {
            var L = previousCandidates.First().Count();
            var hashTree = HashTreeFactory.CreateCandidateTree(L, hashTreePageSize, hashTreeKey);
            hashTree.FillTree(previousCandidates);

            var newCandidates = new List<ushort[]>();
            
            for (int i = 0; i < previousCandidates.Count; i++)
            {
                ushort[] first = previousCandidates[i];

                for (int j = i + 1; j < previousCandidates.Count; j++)
                {
                    var second = previousCandidates[j];
                    
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

                    if (hashTree.GetSupportedSets(newCand).Count() == L + 1 &&
                            !results.Any(x => !x.Except(newCand).Any()))
                    {
                        newCandidates.Add(newCand);
                    }
                    
                }
            }

            return newCandidates;
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