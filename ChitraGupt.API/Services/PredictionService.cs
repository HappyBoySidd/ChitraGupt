﻿using Chitragupt_API.Interfaces;
using Qualytics_L1_ConsoleApp;
using Qualytics_L2_ConsoleApp;
using Qualytics_L3_ConsoleApp;

namespace ChitraGupt.API.Services
{
    public class PredictionService : IPredictionService
    {
        private static readonly float numMinimumAccuracyThreshold = 75;

        public string PredictL1(string strDescription, string strShortDescription, string strL0)
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
                return GetBestSuitedPrediction(sortedScoresWithLabel);
            }
            catch (Exception)
            {
                return "Error: Internal Server Error";
            }
        }

        public string PredictL2(string strDescription, string strShortDescription, string strL0, string strL1)
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
                return GetBestSuitedPrediction(sortedScoresWithLabel);
            }
            catch (Exception)
            {
                return "Error: Internal Server Error";
            }
        }

        public string PredictL3(string strDescription, string strShortDescription, string strL0, string strL1, string strL2)
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
                return GetBestSuitedPrediction(sortedScoresWithLabel);
            }
            catch (Exception)
            {
                return "Error: Internal Server Error";
            }
        }

        public Tuple<string, string, string> PredictReportedCodeValue(string strDescription, string strShortDescription, string strL0)
        {
            try
            {
                L1.ModelInput sampleData1 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                };
                var sortedScoresWithLabel = L1.PredictAllLabels(sampleData1);
                var strL1 = GetBestSuitedPrediction(sortedScoresWithLabel);

                L2.ModelInput sampleData2 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1
                };
                sortedScoresWithLabel = L2.PredictAllLabels(sampleData2);
                var strL2 = GetBestSuitedPrediction(sortedScoresWithLabel);

                L3.ModelInput sampleData3 = new()
                {
                    L0_Level = strL0,
                    Translated_Short_Description = strShortDescription,
                    Translated_Description = strDescription,
                    Reported_Problem_Code_L1 = strL1,
                    Reported_Problem_Code_L2 = strL2
                };
                sortedScoresWithLabel = L3.PredictAllLabels(sampleData3);
                var strL3 = GetBestSuitedPrediction(sortedScoresWithLabel);

                return Tuple.Create(strL1, strL2, strL3);
            }
            catch (Exception)
            {
                return Tuple.Create("Error: Internal Server Error", string.Empty, string.Empty);
            }
        }

        private static string GetBestSuitedPrediction(IOrderedEnumerable<KeyValuePair<string, float>> sortedScoresWithLabel)
        {
            if (sortedScoresWithLabel.Any())
            {
                var strPredictedValue = sortedScoresWithLabel.MaxBy(v => v.Value >= numMinimumAccuracyThreshold).Key;
                if (string.IsNullOrWhiteSpace(strPredictedValue))
                    return "Error: Unable to accurately predict value";
                return strPredictedValue;
            }
            return "Error: Unable to predict value";
        }
    }
}
