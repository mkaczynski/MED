using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class CandidatesGenerator : ICandidatesGenerator
    {
        public IEnumerable<ushort[]>
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

        public IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<ushort[]> results, IEnumerable<Row> transactions)
        {
            var newCandidates = new List<ushort[]>();

            foreach(var c in previousCandidates)
            {
                var head = c.Take(c.Length - 1);
                var tmp = previousCandidates.Where(x => x != c && x.Take(c.Length - 1).SequenceEqual(head)).
                    Select(x => c.Union(x.Skip(c.Length - 1)).ToArray()).ToList();

                tmp.ForEach(Array.Sort);
                
                foreach (var t in tmp)
                {
                    if (!newCandidates.Any(x => x.SequenceEqual(t)) && 
                        !results.Any(x => !x.Except(t).Any()))
                    {
                        newCandidates.Add(t);
                    }
                }
            }

            return newCandidates;
        }
    }
}