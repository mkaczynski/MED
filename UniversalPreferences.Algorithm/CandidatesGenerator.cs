using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;
using UniversalPreferences.HashTree;

namespace UniversalPreferences.Algorithm
{
    public class CandidatesGenerator : ICandidatesGenerator
    {
        public IEnumerable<IEnumerable<ushort>>
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
            res = res.OrderBy(x => x[0]).ToList();
            return res;
        }

        public IEnumerable<IEnumerable<ushort>> GetCandidates(IEnumerable<IEnumerable<ushort>> previousCandidates, IEnumerable<IEnumerable<ushort>> results, IEnumerable<Row> transactions)
        {
            var previousArrays = previousCandidates.Select(x => (ushort[])x).ToList();
            var newCandidates = new List<ushort[]>();
            var L = previousCandidates.First().Count();

            for (int i = 0; i < previousArrays.Count;i++ )
            {
                ushort[] first = previousArrays[i];

                var tmp = new List<ushort[]>();
                for (int j = i+1; j < previousArrays.Count; j++)
                {
                    var second = previousArrays[j];
                    
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
                    newCandidates.Add(newCand);
                }
            }

            var result = new List<IEnumerable<ushort>>();
            var hashTree = HashTreeFactory.CreateCandidateTree(L, 100, 47);
            hashTree.FillTree(previousArrays);

            foreach (var newCandidate in newCandidates)
            {
                if(hashTree.GetSupportedSets(newCandidate).Count() == L+1 &&
                    !results.Any(x => !x.Except(newCandidate).Any()))
                {
                    result.Add(newCandidate);
                }
            }

            return result;
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