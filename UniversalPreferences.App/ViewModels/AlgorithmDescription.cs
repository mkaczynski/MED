using System;
using UniversalPreferences.Algorithm;

namespace UniversalPreferences.App.ViewModels
{
    public class AlgorithmDescription
    {
        public string Name { get; private set; }

        public Func<IAlgorithm> Algorithm { get; private set; }

        public AlgorithmDescription(string name, Func<IAlgorithm> algorithm)
        {
            Name = name;
            Algorithm = algorithm;
        }
    }
}