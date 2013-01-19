using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        IList<ushort[]> FindSetsWhichHasOneElement(IEnumerable<Row> transactions);

        IList<ushort[]> GetCandidates(IList<ushort[]> previousCandidates, IList<ushort[]> results, IEnumerable<Row> transactions);
    }
}