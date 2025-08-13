// See https://aka.ms/new-console-template for more information
using ChitraGupt.Console;

var strFIlePath = "D:\\src\\My Experiments with Truth\\ML\\Qualytics\\BhavishyaVani\\BhavishyaVani\\Data\\VerificationData.csv";
var lstCSVItems = CSVHelper.ReadCSV(strFIlePath);
var isPredictionCompleted = CSVHelper.BuildPredictions(lstCSVItems);

