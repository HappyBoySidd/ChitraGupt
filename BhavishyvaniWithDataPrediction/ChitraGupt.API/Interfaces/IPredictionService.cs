namespace ChitraGupt.API.Interfaces
{
    public interface IPredictionService
    {
        (string, float) PredictL0(string strDescription, string strShortDescription);
        (string, float) PredictL1(string strDescription, string strShortDescription, string strL0);
        (string, float) PredictL2(string strDescription, string strShortDescription, string strL0, string strL1);
        (string, float) PredictL3(string strDescription, string strShortDescription, string strL0, string strL1, string strL2);
        (string strL1, string strL2, string strL3, bool isSuccess) PredictReportedCodeValue(string strDescription, string strShortDescription);
    }
}
