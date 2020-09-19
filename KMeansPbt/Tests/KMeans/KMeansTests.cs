namespace KMeansPbt
{
    public class KMeansTests : BaseKMeansTests
    {
        protected override KMeans GetKMeansTrainer()
        {
            return new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.Random);
        }
    }
}
