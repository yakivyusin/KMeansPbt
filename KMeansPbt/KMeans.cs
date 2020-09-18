using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Linq;

namespace KMeansPbt
{
    public class KMeans
    {
        private readonly KMeansTrainer.InitializationAlgorithm _initializationAlgorithm;

        public KMeans(KMeansTrainer.InitializationAlgorithm initializationAlgorithm)
        {
            _initializationAlgorithm = initializationAlgorithm;
        }

        public Data[] Clusterize(Data[] data, uint k)
        {
            var mlContext = new MLContext();
            var schemaDef = SchemaDefinition.Create(typeof(Data));
            schemaDef["Features"].ColumnType = new VectorDataViewType(NumberDataViewType.Single, data[0].Features.Length);

            var trainingData = mlContext.Data.LoadFromEnumerable(data, schemaDef);
            var pipeline = mlContext.Clustering.Trainers.KMeans(new KMeansTrainer.Options
            {
                FeatureColumnName = "Features",
                InitializationAlgorithm = _initializationAlgorithm,
                NumberOfClusters = (int)k
            });

            var model = pipeline.Fit(trainingData);
            var outputData = model.Transform(trainingData);

            return mlContext.Data
                .CreateEnumerable<Data>(outputData, reuseRowObject: false)
                .ToArray();
        }
    }
}
