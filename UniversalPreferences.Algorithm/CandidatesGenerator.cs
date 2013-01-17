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
            var newCandidates = new List<IEnumerable<ushort>>();

            for (int i = 0; i < previousCandidates.Count();i++ )
            {
                var c = previousCandidates.ElementAt(i);
                var head = c.Take(c.Count() - 1);
                var tmp = previousCandidates.Take(i).Where(x => x.Take(c.Count() - 1).SequenceEqual(head)).
                    Select(x => c.Union(x.Skip(c.Count() - 1)).ToArray()).ToList();

                tmp.ForEach(Array.Sort);
                
                foreach (var t in tmp)
                {
                    var subsets = GetSubsets(t);

                    if (!results.Any(x => !x.Except(t).Any()) &&
                        subsets.All(x => previousCandidates.Any(p => p.SequenceEqual(x))))
                    //if (!results.Any(x => x.Intersect(t).Any()))
                    {
                        newCandidates.Add(t);
                    }
                }
            }

            return newCandidates;
        }

        private IEnumerable<ushort[]> GetSubsets(IEnumerable<ushort> set)
        {
            var subsets = set.Select((t, i) => set.Take(i).Concat(set.Skip(i + 1)).ToArray()).ToList();
            subsets.ForEach(x=>Array.Sort(x));
            return subsets;
        }
    }
}