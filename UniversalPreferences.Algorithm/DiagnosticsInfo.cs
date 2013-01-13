using System;

namespace UniversalPreferences.Algorithm
{
    public class DiagnosticsInfo: EventArgs
    {
        public string Info { get; private set; }

        public DiagnosticsInfo(string info)
        {
            Info = info;
        }
    }
}