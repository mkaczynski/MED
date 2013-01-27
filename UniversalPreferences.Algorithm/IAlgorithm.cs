using System;
using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface IAlgorithm
    {
        event EventHandler<DiagnosticsInfo> DiagnosticsEvent;

        IEnumerable<SimpleRow> FindPreferences(IEnumerable<Row> transactions); // byc moze bedzie zwracac w formie listy obiektow a nie listy tablic
    }
}