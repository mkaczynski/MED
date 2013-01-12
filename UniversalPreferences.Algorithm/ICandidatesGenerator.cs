using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        IEnumerable<ushort[]> FindSetsWhichHasOneElement(IEnumerable<Row> transactions);

        IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<ushort[]> results, IEnumerable<Row> transactions);
    }
}