using Kartverket.ReportGenerator.App_Start;
using Kartverket.ReportGenerator.Services;
using System;
using System.Collections.Generic;
using System.Web.Http.Cors;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kartverket.ReportGenerator.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiRootController : ApiController
    {
        private readonly IStatisticsService _statisticsService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApiRootController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        [Route("api/statistics")]
        public List<ReportMeasurement> Get(string organization)
        {
            try
            {
               return _statisticsService.GetReportLiveSummary(organization);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return new List<ReportMeasurement>();
        }

        [Authorize(Roles = AuthConfig.DatasetProviderRole)]
        [HttpGet]
        [HttpPost]
        [Route("api/internal/create-statistics")]
        public void Post()
        {
            try
            {
                _statisticsService.CreateReport();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

    }
}
