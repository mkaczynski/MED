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

        public IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<Row> transactions)
        {
            var newCandidates = new List<ushort[]>();

            //previousCandidates = new List<ushort[]> { new ushort[] { 1, 2 }, new ushort[] { 1, 3 }, 
            //    new ushort[] { 1, 4 }, new ushort[] { 2, 4 }  };

            foreach(var c in previousCandidates)
            {
                var head = c.Take(c.Length - 1);
                var tmp = previousCandidates.Where(x => x != c && x.Take(c.Length - 1).SequenceEqual(head)).
                    Select(x => c.Union(x.Skip(c.Length - 1)).ToArray()).ToList();

                tmp.ForEach(Array.Sort);
                
                foreach (var t in tmp)
                {
                    if (!newCandidates.Any(x => x.SequenceEqual(t)))
                    {
                        newCandidates.Add(t);
                    }
                }
            }

            ////tutaj chyba mozna wykorzystac drzewo
            //foreach (var transaction in transactions)
            //{
            //    foreach (var candidate in previousCandidates)
            //    {
            //        if (FakeHashTree.IsItemsetSupported(candidate, transaction))
            //        {
            //            var candidates = GenerateCandidates(candidate, transaction);
            //            foreach (var c in candidates)
            //            {
            //                if (!newCandidates.Any(x => x.SequenceEqual(c)))
            //                {
            //                    newCandidates.Add(c);
            //                }
            //            }
            //        }
            //    }
            //}

            return newCandidates;
        }

        ////todo: zoptymalizowac/zrefaktoryzowac
        //private IEnumerable<ushort[]> GenerateCandidates(ushort[] candidate, Row transaction)
        //{
        //    var res = new List<ushort[]>();

        //    for (int i = 0; i < transaction.Attributes.Length; ++i)
        //    {
        //        if (!transaction.Attributes[i].HasValue)
        //        {
        //            continue;
        //        }

        //        var @continue = false;
        //        for (int j = 0; j < candidate.Length; ++j)
        //        {
        //            if (candidate[j] == transaction.Attributes[i].Value)
        //            {
        //                @continue = true;
        //                continue;
        //            }
        //        }

        //        if (@continue)
        //        {
        //            continue;
        //        }

        //        var newCandidate = new ushort[candidate.Length + 1];
        //        for (int j = 0; j < candidate.Length; ++j)
        //        {
        //            newCandidate[j] = candidate[j];
        //        }
        //        newCandidate[candidate.Length] = transaction.Attributes[i].Value;
        //        Array.Sort(newCandidate);
        //        res.Add(newCandidate);
        //    }

        //    return res;
        //}
    }
}