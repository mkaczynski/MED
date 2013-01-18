using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

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
            return res;
        }

        public IEnumerable<IEnumerable<ushort>> GetCandidates(IEnumerable<IEnumerable<ushort>> previousCandidates, IEnumerable<IEnumerable<ushort>> results, IEnumerable<Row> transactions)
        {
            var sorted = previousCandidates.Select(x => x.ToArray()).OrderBy(x => x, new ArrayComparer()).ToList();
            var newCandidates = new List<IEnumerable<ushort>>();
            var L = previousCandidates.First().Count();

            for (int i = 0; i < sorted.Count;i++ )
            {
                ushort[] first = sorted[i];

                var tmp = new List<ushort[]>();
                for (int j = i+1; j < sorted.Count; j++)
                {
                    var second = sorted[j];
                    
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
                    tmp.Add(newCand);
                }
                
                foreach (var t in tmp)
                {
                    if (!results.Any(x => !x.Except(t).Any()))
                    {
                        newCandidates.Add(t);
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

        class ArrayComparer : IComparer<ushort[]>
        {
            public int Compare(ushort[] x, ushort[] y)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if(x[i] < y[i])
                    {
                        return -1;
                    }
                    if (x[i] > y[i])
                    {
                        return 1;
                    }
                }
                return 0;
            }
        }
    }
}