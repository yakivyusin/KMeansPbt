using Microsoft.ML.Data;

namespace KMeansPbt
{
    public class Data
    {
        public float[] Features { get; set; }

        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}
