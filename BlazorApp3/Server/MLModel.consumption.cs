﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;

namespace BlazorApp3.Server
{
    public partial class MLModel
    {
        /// <summary>
        /// model input class for MLModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"review")]
#pragma warning disable CS8618 // Non-nullable property 'Review' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
            public string Review { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Review' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

            [ColumnName(@"sentiment")]
#pragma warning disable CS8618 // Non-nullable property 'Sentiment' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
            public string Sentiment { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Sentiment' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        }

        #endregion

        /// <summary>
        /// model output class for MLModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
#pragma warning disable CS8618 // Non-nullable property 'Prediction' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
            public string Prediction { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Prediction' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable property 'Score' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
            public float[] Score { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Score' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        }

        #endregion

        private static readonly string MLNetModelPath = Path.GetFullPath("MLModel.zip");

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            MLContext mlContext = new MLContext();

            // Load model & create prediction engine
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out DataViewSchema modelInputSchema);
            PredictionEngine<ModelInput, ModelOutput> predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            ModelOutput result = predEngine.Predict(input);
            return result;
        }
    }
}
