using System.Collections.Generic;

namespace Kartverket.ReportApi
{
    public class ReportResultData
    {
        public string Label;
        public int TotalDataCount;
        public List<ReportResultDataValue> Values;
    }
}
