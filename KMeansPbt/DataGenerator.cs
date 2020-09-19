using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KMeansPbt
{
    public class DataGenerator
    {
        private const int FeatureDimension = 3;
        private readonly Random _random = new Random();

        public Data[] Generate(uint unique, uint dup = 0)
        {
            var data = GenerateUniquePoints(unique).ToList();

            for (int i = 0; i < dup; i++)
            {
                var copy = new Data
                {
                    Features = (float[])data.Last().Features.Clone()
                };

                data.Add(copy);
            }

            return data.ToArray();
        }

        private Data[] GenerateUniquePoints(uint count)
        {
            var set = new HashSet<Data>(new DataComparer());

            while (set.Count != count)
            {
                var features = new float[FeatureDimension];

                for (int i = 0; i < features.Length; i++)
                {
                    features[i] = (float)_random.NextDouble();
                }

                set.Add(new Data
                {
                    Features = features
                });
            }

            return set.ToArray();
        }

        private class DataComparer : IEqualityComparer<Data>
        {
            public bool Equals([AllowNull] Data x, [AllowNull] Data y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                return x.Features.SequenceEqual(y.Features);
            }

            public int GetHashCode([DisallowNull] Data obj)
            {
                var hashCode = 352033288;

                foreach (var feature in obj.Features)
                {
                    hashCode = hashCode * -1521134295 + feature.GetHashCode();
                }

                return hashCode;
            }
        }
    }
}
