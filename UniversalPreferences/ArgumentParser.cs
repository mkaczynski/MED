using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences
{
    public class ArgumentParser
    {
        private readonly IList<string> args;

        public ArgumentParser(string[] args)
        {
            this.args = args;
        }

        public Arguments ParseArguments()
        {
            var result = new Arguments();

            var dataFile = FindKeyPair("data");
            CheckFileExists(dataFile);
            result.DataFilePath = dataFile;

            var relations = FindKeyPair("relations");
            CheckFileExists(relations);
            result.RelationsFilePath = relations;

            var delim = FindKeyPair("delim");
            result.Delimiter = delim;

            var classIndex = FindKeyPair("index");
            result.ClassIndex = ParseIntager(classIndex);

            var hashTreePageSize = FindKeyPairOrDefault("pageSize", "100");
            result.HashTreePageSize = ParseIntager(hashTreePageSize);
            CheckValue(result.HashTreePageSize, "pageSize");

            var key = FindKeyPairOrDefault("key", "47");
            result.HashTreeFirstNumber = ParseIntager(key);
            CheckValue(result.HashTreeFirstNumber, "pageSize");

            var relation = FindKeyPairOrDefault("relationType", "NonStrict");
            result.RelationKind = ParseRelation(relation);

            var write = FindKeyPairOrDefault("write", "true");
            bool w;
            if (bool.TryParse(write, out w))
            {
                result.WriteIterationResults = w;
            }
            else
            {
                result.WriteIterationResults = true;
            }

            var method = FindKeyPairOrDefault("method", "T");
            CheckMethod(method);
            result.Method = method;

            var preferenceMatrix = FindKeyPairOrDefault("preferenceMatrix", null);
            result.PreferenceMatrix = preferenceMatrix;

            return result;
        }

        private void CheckMethod(string method)
        {
            if(method != "T" && method != "P" && method != "G")
                throw new ArgumentException("Dopuszczalne metody to: T - Terleckiego, G - z Generatorem, P - wersja podstawowa");
        }

        private RelationKind ParseRelation(string relation)
        {
            RelationKind result;
            if(!Enum.TryParse(relation, true, out result))
                throw new ArgumentException(string.Format(
                    "Niepoprawny format relacji {0}, dostępne wartości: Strict, NonStrict, Equals.", relation));

            return result;
        }

        private void CheckValue(int val, string key)
        {
            if(val < 0)
                throw new ArgumentException(string.Format("Wartość {0} musi być większa od 0.", key));
        }

        private string FindKeyPairOrDefault(string key, string @default)
        {
            var keyPair = args.FirstOrDefault(x => x.StartsWith(key));
            if (keyPair == null)
            {
                return @default;
            }

            var splitted = keyPair.Split(':');
            if (splitted.Length != 2)
            {
                throw new ArgumentException(string.Format(
                    "Argument {0} jest w niepoprawnym formacie, prawidłowy format key:value.", keyPair));
            }

            return splitted[1];
        }

        private void CheckFileExists(string file)
        {
            if(!File.Exists(file))
                throw new ArgumentException(string.Format("Plik {0} nie istnieje.", file));
        }

        private int ParseIntager(string value)
        {
            int result;
            if (!int.TryParse(value, out result))
            {
                throw new ArgumentException(string.Format("{0} nie jest poprawną liczbą naturalną", value));
            }

            return result;
        }

        private string FindKeyPair(string key)
        {
            var keyPair = args.FirstOrDefault(x => x.StartsWith(key));
            if (keyPair == null)
            {
                throw new ArgumentException(string.Format("Brak wymaganego argumentu {0}", key));
            }

            var splitted = keyPair.Split(':');
            if (splitted.Length != 2)
            {
                throw new ArgumentException(string.Format(
                    "Argument {0} jest w niepoprawnym formacie, prawidłowy format key:value.", keyPair));
            }

            return splitted[1];
        }
    }
}