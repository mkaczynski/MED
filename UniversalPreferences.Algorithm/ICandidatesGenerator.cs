﻿using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public interface ICandidatesGenerator
    {
        IEnumerable<ushort[]> GetCandidates(IEnumerable<ushort[]> previousCandidates, IEnumerable<Row> transactions);
    }
}