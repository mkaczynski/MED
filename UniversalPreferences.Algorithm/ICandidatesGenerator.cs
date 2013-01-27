using System;
using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        void Initialize(Func<SimpleRow, int> func);

        IList<SimpleRow> FindSetsWhichHasOneElement(IEnumerable<Row> transactions);

        IList<SimpleRow> GetCandidates(IList<SimpleRow> previousCandidates, IList<SimpleRow> results, IEnumerable<Row> transactions);
    }
}