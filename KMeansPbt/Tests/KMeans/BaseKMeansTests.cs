using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;

namespace KMeansPbt
{
    public abstract class BaseKMeansTests
    {
        private readonly KMeans _kMeans;
        private readonly DataGenerator _dataGenerator;

        public BaseKMeansTests()
        {
            _kMeans = GetKMeansTrainer();
            _dataGenerator = new DataGenerator();
        }

        protected abstract KMeans GetKMeansTrainer();

        [Property]
        public Property DataPoints_Less_Than_K(PositiveInt unique)
        {
            var data = _dataGenerator.Generate((uint)unique.Get);

            return Prop.Throws<InvalidOperationException, bool>(new Lazy<bool>(() =>
            {
                var result = _kMeans.Clusterize(data, (uint)unique.Get + 1);
                return true;
            }));
        }

        [Property]
        public Property UniqueDataPoints_Less_Than_K(PositiveInt unique, PositiveInt dup)
        {
            var data = _dataGenerator.Generate((uint)unique.Get, (uint)dup.Get);

            return Prop.Throws<InvalidOperationException, bool>(new Lazy<bool>(() =>
            {
                var result = _kMeans.Clusterize(data, (uint)unique.Get + 1);
                return true;
            }));
        }

        [Property]
        public Property Correct_K_Clusters(PositiveInt unique, NonNegativeInt dup, PositiveInt k)
        {
            var data = _dataGenerator.Generate((uint)unique.Get, (uint)dup.Get);

            Func<bool> property = () =>
            {
                var result = _kMeans.Clusterize(data, (uint)k.Get);

                return result.Select(x => x.PredictedClusterId).Distinct().Count() == k.Get;
            };

            return property.When(unique.Get >= k.Get);
        }

        [Property]
        public Property SamePoints_In_SameCluster(PositiveInt unique, PositiveInt dup, PositiveInt k)
        {
            var data = _dataGenerator.Generate((uint)unique.Get, (uint)dup.Get);

            Func<bool> property = () =>
            {
                var result = _kMeans.Clusterize(data, (uint)k.Get);
                var samePoints = result.Skip(unique.Get - 1).ToList();

                return samePoints.Select(x => x.PredictedClusterId).Distinct().Count() == 1;
            };

            return property.When(unique.Get >= k.Get);
        }

        [Property]
        public Property Check_EuclideMetric(PositiveInt unique, PositiveInt k)
        {
            var data = _dataGenerator.Generate((uint)unique.Get);

            Func<bool> property = () =>
            {
                var result = _kMeans.Clusterize(data, (uint)k.Get);

                return result.All(x1 => Array.IndexOf(x1.Distances, x1.Distances.Min()) == x1.PredictedClusterId - 1);
            };

            return property.When(unique.Get >= k.Get);
        }
    }
}
