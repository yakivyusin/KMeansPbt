using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;

namespace KMeansPbt
{
    public class KMeansYinyangTests
    {
        [Property]
        public Property DataPoints_Less_Than_K(PositiveInt unique)
        {
            var generator = new DataGenerator();
            var kmeans = new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
            var data = generator.Generate((uint)unique.Get);

            return Prop.Throws<InvalidOperationException, bool>(new Lazy<bool>(() =>
            {
                var result = kmeans.Clusterize(data, (uint)unique.Get + 1);
                return true;
            }));
        }

        [Property]
        public Property UniqueDataPoints_Less_Than_K(PositiveInt unique, PositiveInt dup)
        {
            var generator = new DataGenerator();
            var kmeans = new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
            var data = generator.Generate((uint)unique.Get, (uint)dup.Get);

            return Prop.Throws<InvalidOperationException, bool>(new Lazy<bool>(() =>
            {
                var result = kmeans.Clusterize(data, (uint)unique.Get + 1);
                return true;
            }));
        }

        [Property]
        public Property Correct_K_Clusters(PositiveInt unique, NonNegativeInt dup, PositiveInt k)
        {
            var generator = new DataGenerator();
            var kmeans = new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
            var data = generator.Generate((uint)unique.Get, (uint)dup.Get);

            Func<bool> property = () =>
            {
                var result = kmeans.Clusterize(data, (uint)k.Get);

                return result.Select(x => x.PredictedClusterId).Distinct().Count() == k.Get;
            };

            return property.When(unique.Get >= k.Get);
        }

        [Property]
        public Property SamePoints_In_SameCluster(PositiveInt unique, PositiveInt dup, PositiveInt k)
        {
            var generator = new DataGenerator();
            var kmeans = new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
            var data = generator.Generate((uint)unique.Get, (uint)dup.Get);

            Func<bool> property = () =>
            {
                var result = kmeans.Clusterize(data, (uint)k.Get);
                var samePoints = result.Skip(unique.Get - 1).ToList();

                return samePoints.Select(x => x.PredictedClusterId).Distinct().Count() == 1;
            };

            return property.When(unique.Get >= k.Get);
        }

        [Property]
        public Property Check_EuclideMetric(PositiveInt unique, PositiveInt k)
        {
            var generator = new DataGenerator();
            var kmeans = new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
            var data = generator.Generate((uint)unique.Get);

            Func<bool> property = () =>
            {
                var result = kmeans.Clusterize(data, (uint)k.Get);

                return result.All(x1 => Array.IndexOf(x1.Distances, x1.Distances.Min()) == x1.PredictedClusterId - 1);
            };

            return property.When(unique.Get >= k.Get);
        }
    }
}
