using ChitraGupt.API.Interfaces;
using Qualytics_L0_ConsoleApp;
using Qualytics_L1_ConsoleApp;
using Qualytics_L2_ConsoleApp;
using Qualytics_L3_ConsoleApp;

namespace ChitraGupt.API.Services
{
    public class PredictionService : IPredictionService
    {
        #region Private declarations
        private static float _numMinimumAccuracyThreshold;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public PredictionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _numMinimumAccuracyThreshold = _configuration.GetValue<float>("numMinimumAccuracyThreshold");
        }
        public PredictionService()
        {

            _numMinimumAccuracyThreshold = 60;
        }
        #endregion

        #region Public methods
        public (string, float) PredictL0(string strDescription, string strShortDescription)
        {
            try
            {
                L0.ModelInput sampleData = new()
                {
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                };
                var sortedScoresWithLabel = L0.PredictAllLabels(sampleData);
                var (strPredictedValue, numPercentAccuracy, ex) = SanitizePredictedValues(sortedScoresWithLabel);
                return (strPredictedValue, numPercentAccuracy);
            }
            catch (Exception)
            {
                return ("Error: Internal Server Error", 0.0f);
            }
        }

        public (string, float) PredictL1(string strDescription, string strShortDescription, string strL0)
        {
            try
            {
                L1.ModelInput sampleData = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                };
                var sortedScoresWithLabel = L1.PredictAllLabels(sampleData);
                var (strPredictedValue, numPercentAccuracy, ex) = SanitizePredictedValues(sortedScoresWithLabel);
                return (strPredictedValue, numPercentAccuracy);
            }
            catch (Exception)
            {
                return ("Error: Internal Server Error", 0.0f);
            }
        }

        public (string, float) PredictL2(string strDescription, string strShortDescription, string strL0, string strL1)
        {
            try
            {
                L2.ModelInput sampleData = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1
                };
                var sortedScoresWithLabel = L2.PredictAllLabels(sampleData);
                var (strPredictedValue, numPercentAccuracy, ex) = SanitizePredictedValues(sortedScoresWithLabel);
                return (strPredictedValue, numPercentAccuracy);
            }
            catch (Exception)
            {
                return ("Error: Internal Server Error", 0.0f);
            }
        }

        public (string, float) PredictL3(string strDescription, string strShortDescription, string strL0, string strL1, string strL2)
        {
            try
            {
                L3.ModelInput sampleData = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1,
                    Reported_Problem_Code_L2 = strL2
                };
                var sortedScoresWithLabel = L3.PredictAllLabels(sampleData);
                var (strPredictedValue, numPercentAccuracy, ex) = SanitizePredictedValues(sortedScoresWithLabel);
                return (strPredictedValue, numPercentAccuracy);
            }
            catch (Exception)
            {
                return ("Error: Internal Server Error", 0.0f);
            }
        }

        public (string strL1, string strL2, string strL3, bool isSuccess) PredictReportedCodeValue(string strDescription, string strShortDescription)
        {
            var isSuccess = false;
            string strL1PredictedValue, strL2PredictedValue, strL3PredictedValue;
            strL1PredictedValue = strL2PredictedValue = strL3PredictedValue = string.Empty;
            try
            {
                L0.ModelInput sampleData = new()
                {
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                };
                var sortedScoresWithLabel = L0.PredictAllLabels(sampleData);
                var (strL0, numL0Accuracy, exL0) = SanitizePredictedValues(sortedScoresWithLabel);

                L1.ModelInput sampleData1 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                };
                sortedScoresWithLabel = L1.PredictAllLabels(sampleData1);
                var (strL1, numL1Accuracy, exL1) = SanitizePredictedValues(sortedScoresWithLabel);
                strL1PredictedValue = strL1;

                L2.ModelInput sampleData2 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1
                };
                sortedScoresWithLabel = L2.PredictAllLabels(sampleData2);
                var (strL2, numL2Accuracy, exL2) = SanitizePredictedValues(sortedScoresWithLabel);
                strL2PredictedValue = strL2;

                L3.ModelInput sampleData3 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1,
                    Reported_Problem_Code_L2 = strL2
                };
                sortedScoresWithLabel = L3.PredictAllLabels(sampleData3);
                var (strL3, numL3Accuracy, exL3) = SanitizePredictedValues(sortedScoresWithLabel);
                strL3PredictedValue = strL3;

                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return (strL1PredictedValue, strL2PredictedValue, strL3PredictedValue, isSuccess);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the algorithm name with the highest accuracy, it's accuracy % and an indicator to portray if the accuracy is beyond a threshold
        /// </summary>
        /// <param name="sortedScoresWithLabel">Collection of algorithm names and their corresponding accuracy values</param>
        /// <returns>Returns the algorithm name with the highest accuracy, it's accuracy % and an indicator to portray if the accuracy is beyond a threshold</returns>
        private static (string strPredictedValue, float numAccuracy, bool isAccurate) GetBestSuitedPrediction(IOrderedEnumerable<KeyValuePair<string, float>> sortedScoresWithLabel)
        {
            var strPredictedValue = string.Empty;
            var numPercentAccuracy = 0.0f;
            if (sortedScoresWithLabel.Any())
            {
                var objPredictedValue = sortedScoresWithLabel.MaxBy(v => v.Value);
                numPercentAccuracy = objPredictedValue.Value * 100;
                strPredictedValue = objPredictedValue.Key;
                bool isAccurate = false;
                
                isAccurate = numPercentAccuracy >= _numMinimumAccuracyThreshold;
                return (strPredictedValue, numPercentAccuracy, isAccurate);
            }
            return (strPredictedValue, numPercentAccuracy, false);
        }

        /// <summary>
        /// Gets the predicted value with the highest accuracy and sanitizes the prediction based on the predefined accuracy threshold 
        /// </summary>
        /// <param name="sortedScoresWithLabel">Collection of algorithm names and their corresponding accuracy values</param>
        /// <returns>Returns the algorithm name with the highest accuracy along with the accuracy %, if accuracy meets  the threshold else an error</returns>
        /// <exception cref="InvalidDataException">Exception thrown when accuracy is less than expected threshold</exception>
        private static (string, float, Exception?) SanitizePredictedValues(IOrderedEnumerable<KeyValuePair<string, float>> sortedScoresWithLabel)
        {
            var objPrediction = GetBestSuitedPrediction(sortedScoresWithLabel);
            Exception? exDetails = null;

            if (!objPrediction.isAccurate)
                exDetails = new InvalidDataException("Unable to fetch accurate value");
            return (objPrediction.strPredictedValue, objPrediction.numAccuracy, exDetails);
        }
        #endregion
    }
}
