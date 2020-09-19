namespace KMeansPbt
{
    public class KMeansYinyangTests : BaseKMeansTests
    {
        protected override KMeans GetKMeansTrainer()
        {
            return new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansYinyang);
        }
    }
}
