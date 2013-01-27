using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        IList<SimpleRow> FindSetsWhichHasOneElement(IEnumerable<Row> transactions);

        IList<SimpleRow> GetCandidates(IList<SimpleRow> previousCandidates, IList<SimpleRow> results, IEnumerable<Row> transactions);
    }
}