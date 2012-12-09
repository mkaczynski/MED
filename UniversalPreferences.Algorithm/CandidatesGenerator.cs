using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class CandidatesGenerator : ICandidatesGenerator
    {
        public IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<Row> transactions)
        {
            var newCandidates = new List<ushort[]>();

            //tutaj chyba mozna wykorzystac drzewo
            foreach (var transaction in transactions)
            {
                foreach (var candidate in previousCandidates)
                {
                    if (Helper.IsItemsetSupported(candidate, transaction))
                    {
                        var candidates = GenerateCandidates(candidate, transaction);
                        foreach (var c in candidates)
                        {
                            if (!newCandidates.Any(x => x.SequenceEqual(c)))
                            {
                                newCandidates.Add(c);
                            }
                        }
                    }
                }
            }

            return newCandidates;
        }

        //todo: zoptymalizowac/zrefaktoryzowac
        private IEnumerable<ushort[]> GenerateCandidates(ushort[] candidate, Row transaction)
        {
            var res = new List<ushort[]>();

            for (int i = 0; i < transaction.Attributes.Length; ++i)
            {
                if (!transaction.Attributes[i].HasValue)
                {
                    continue;
                }

                var @continue = false;
                for (int j = 0; j < candidate.Length; ++j)
                {
                    if (candidate[j] == transaction.Attributes[i].Value)
                    {
                        @continue = true;
                        continue;
                    }
                }

                if (@continue)
                {
                    continue;
                }

                var newCandidate = new ushort[candidate.Length + 1];
                for (int j = 0; j < candidate.Length; ++j)
                {
                    newCandidate[j] = candidate[j];
                }
                newCandidate[candidate.Length] = transaction.Attributes[i].Value;
                Array.Sort(newCandidate);
                res.Add(newCandidate);
            }

            return res;
        }
    }
}