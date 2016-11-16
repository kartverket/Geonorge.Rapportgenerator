using System.Collections.Generic;
using Kartverket.ReportApi;
using Kartverket.ReportGenerator.Models;

namespace Kartverket.ReportGenerator.Services
{
    public interface IReportService
    {
        ReportResult GetQueryResult(ReportQuery rq);
        QueryConfig GetQueries();
    }
}