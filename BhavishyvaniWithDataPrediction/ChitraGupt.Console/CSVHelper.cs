using ChitraGupt.Console.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitraGupt.Console
{
    class CSVHelper
    {
        internal static IEnumerable ReadCSV(string strFileName)
        {
            var lstInputData = new List<ReportedIssue>();
            using (TextFieldParser csvParser = new TextFieldParser(strFileName))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[]? fields = csvParser.ReadFields();
                    if (fields == null) continue;
                    lstInputData.Add(new()
                    {
                        L0 = fields[0],
                        Description = fields[1],
                        ShortDescription = fields[2]
                    });
                }
            }
            return lstInputData;
        }

        internal static bool BuildPredictions(IEnumerable lstBaseValues)
        {
            return false;
        }

        internal static Tuple<string, string, string> PredictValue(string strDescripton, string strShortDescription, string strL0)
        {
            return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
        }
    }
}
