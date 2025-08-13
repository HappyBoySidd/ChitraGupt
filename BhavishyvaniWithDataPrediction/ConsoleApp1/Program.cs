// See https://aka.ms/new-console-template for more information
using System;
using ClosedXML.Excel;
using ChitraGupt.API.Services; // Assuming the concrete class is here
using ChitraGupt.API.Interfaces;


namespace ExcelPredictionProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var MlPath = Path.GetDirectoryName(Environment.CurrentDirectory);

            string inputPath = @"C:\Datatoclean\Verification.xlsx";
            string outputPath = @"C:\Datatoclean\Verification.xlsx";

            // Instantiate your actual service (implementing IPredictionService)
            IPredictionService predictionService = new PredictionService(); // <-- adjust class name if needed

            using var workbook = new XLWorkbook(inputPath);
            var worksheet = workbook.Worksheet(1); // First sheet

            int headerRow = 1;
            int lastCol = worksheet.LastColumnUsed().ColumnNumber();

            // Get relevant column numbers by header names
            var shortDescCol = GetColumnByHeader(worksheet, "Short Description");
            var descCol = GetColumnByHeader(worksheet, "Description");

            // Add prediction columns
            int l1Col = lastCol + 1;
            int l2Col = lastCol + 2;
            int l3Col = lastCol + 3;

            worksheet.Cell(headerRow, l1Col).Value = "Predicted Problem Code L1";
            worksheet.Cell(headerRow, l2Col).Value = "Predicted Problem Code L2";
            worksheet.Cell(headerRow, l3Col).Value = "Predicted Problem Code L3";

            // Loop through data
            for (int row = headerRow + 1; row <= worksheet.LastRowUsed().RowNumber(); row++)
            {
                string shortDesc = worksheet.Cell(row, shortDescCol).GetString().Trim();
                string description = worksheet.Cell(row, descCol).GetString().Trim();

                if (string.IsNullOrWhiteSpace(shortDesc) && string.IsNullOrWhiteSpace(description))
                    continue;

                var (L1, L2, L3, success) = predictionService.PredictReportedCodeValue(description, shortDesc);

                worksheet.Cell(row, l1Col).Value = success ? L1 : "Error";
                worksheet.Cell(row, l2Col).Value = success ? L2 : "Error";
                worksheet.Cell(row, l3Col).Value = success ? L3 : "Error";
            }

            workbook.SaveAs(outputPath);
            Console.WriteLine($" Prediction complete. Output saved at: {outputPath}");
        }

        // Helper method to find column number by header text
        static int GetColumnByHeader(IXLWorksheet worksheet, string headerText)
        {
            foreach (var cell in worksheet.Row(1).CellsUsed())
            {
                if (cell.GetValue<string>().Trim().Equals(headerText, StringComparison.OrdinalIgnoreCase))
                    return cell.Address.ColumnNumber;
            }
            throw new Exception($"Column with header '{headerText}' not found.");
        }
    }
}
