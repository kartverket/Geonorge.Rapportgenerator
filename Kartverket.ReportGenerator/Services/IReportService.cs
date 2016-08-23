using Kartverket.ReportApi;

namespace Kartverket.ReportGenerator.Services
{
    public interface IReportService
    {
        ReportResult GetQueryResult(ReportQuery rq);
    }
}