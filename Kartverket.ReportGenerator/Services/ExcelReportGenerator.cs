using Kartverket.ReportApi;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Services
{
    public class ExcelReportGenerator
    {
        private ExcelWorksheet ws;
        private int RowIndex = 1;
        private int ColumnIndex = 1;
        Dictionary<string, string> _codeList;

        public Stream CreateExcelSheet(ReportQuery reportQuery, ReportResult result, Dictionary<string, string> codeList)
        {
            _codeList = codeList;
            RowIndex = 1;
            ColumnIndex = 1;

            var excelPackage = new ExcelPackage();
            ws = excelPackage.Workbook.Worksheets.Add("Report");
            AddHeadlinesToSheet(reportQuery);
            AddContentToSheet(result, reportQuery);
            excelPackage.Save();

            var fileStream = excelPackage.Stream;
            fileStream.Flush();
            fileStream.Position = 0;

            return fileStream;
        }

        private void AddContentToSheet(ReportResult result, ReportQuery reportQuery)
        {
            foreach (var data in result.Data.ToList())
            {
                NewRow();
                AddContent(data.Label);
                AddContent(GetLabelNumber(data));
                AddContent(data.Values[0].Key);
                AddContent(reportQuery.QueryName);

                if (!string.IsNullOrEmpty(data.Values[0].Value) && IsNumeric(data.Values[0].Value))
                    AddContent(Convert.ToInt16(data.Values[0].Value), "0");
                else
                    AddContent(data.Values[0].Value);

                if (!string.IsNullOrEmpty(data.Values[1].Value) && IsNumeric(data.Values[1].Value))
                    AddContent(Convert.ToInt16(data.Values[1].Value), "0");
                else
                    AddContent(data.Values[1].Value);

                if (data.TotalDataCount > 0)
                    AddContent(Convert.ToInt16(data.TotalDataCount), "0");
                
                if (data.TotalDataCount > 0)
                    AddContent((Math.Truncate((Convert.ToDecimal(data.Values[0].Value) / data.TotalDataCount) * 100)), "0");
                
                if (data.Values.Count > 2 && data.Values[2].Key == "Bekreftet")
                    AddContent(data.Values[2].Value);

                if (data.Values.Count > 2 && data.Values[4].Key == "Status")
                    AddContent(GetDOKStatus(data.Values[4].Value));
            }
        }

        private string GetLabelNumber(ReportResultData data)
        {
            string label = "";

            if (data.Values.Count > 2 && data.Values[2].Key == "Number")
                return data.Values[2].Value;

            if (data.Values.Count > 3 && data.Values[3].Key == "Number")
                return data.Values[3].Value;

            label = data.Label;
            label = label.Replace(" kommune", "");

            if (_codeList.ContainsValue(label))
                return _codeList.Where(d => d.Value == label).Select(s => s.Key).FirstOrDefault();

            return label;
        }

        private void AddHeadlinesToSheet(ReportQuery reportQuery)
        {
            AddContent("Område");
            AddContent("Nummer");
            AddContent("Data");
            AddContent("Spørring");
            if (reportQuery.QueryName == "register-DOK-coverage")
                AddContent("Dekning");
            else
                AddContent("Antall");
            if (reportQuery.QueryName == "register-DOK-coverage")
                AddContent("Valgt");
            else
                AddContent("Tillegg");

            AddContent("Totalt");
            AddContent("% av totalt");

            if (reportQuery.QueryName == "register-DOK-selectedAndAdditional")
            {
                AddContent("Oppdatert");
                AddContent("Status");
            }
        }


        private void AddContent(object content, string format = null)
        {
            ws.Cells[RowIndex, ColumnIndex].Style.Numberformat.Format = !string.IsNullOrEmpty(format) ? format : "";
            ws.Cells[RowIndex, ColumnIndex++].Value = content;
        }

        private void NewRow()
        {
            RowIndex++;
            ColumnIndex = 1;
        }

        private static string GetDOKStatus(string status)
        {
            switch (status)
            {
                case "valid":
                    return "Utført";
                case "draft":
                    return "I prosess";
                default:
                    return "Ikke startet";
            }
        }

        private static bool IsNumeric(object data)
        {
            decimal number;
            bool canConvert = decimal.TryParse(data.ToString(), out number);
            if (canConvert)
                return true;
            else
                return false;
        }
    }
}