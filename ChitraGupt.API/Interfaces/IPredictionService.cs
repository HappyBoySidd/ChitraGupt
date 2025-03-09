namespace ChitraGupt.API.Interfaces
{
    public interface IPredictionService
    {
        string PredictL1(string strDescription, string strShortDescription, string strL0);
        string PredictL2(string strDescription, string strShortDescription, string strL0, string strL1);
        string PredictL3(string strDescription, string strShortDescription, string strL0, string strL1, string strL2);
        (string strL1, string strL2, string strL3, string strStatus) PredictReportedCodeValue(string strDescription, string strShortDescription, string strL0);
    }
}
