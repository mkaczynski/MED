using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        IEnumerable<IEnumerable<ushort>> FindSetsWhichHasOneElement(IEnumerable<Row> transactions);

        IEnumerable<IEnumerable<ushort>> GetCandidates(IEnumerable<IEnumerable<ushort>> previousCandidates, IEnumerable<IEnumerable<ushort>> results, IEnumerable<Row> transactions);
    }
}