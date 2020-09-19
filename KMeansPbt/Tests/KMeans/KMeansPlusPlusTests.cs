namespace KMeansPbt
{
    public class KMeansPlusPlusTests : BaseKMeansTests
    {
        protected override KMeans GetKMeansTrainer()
        {
            return new KMeans(Microsoft.ML.Trainers.KMeansTrainer.InitializationAlgorithm.KMeansPlusPlus);
        }
    }
}
