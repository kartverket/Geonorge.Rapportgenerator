using Kartverket.ReportGenerator.App_Start;
using Kartverket.ReportGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kartverket.ReportGenerator.Controllers
{
    [Authorize(Roles = AuthConfig.DatasetProviderRole)]
    public class ApiRootController : ApiController
    {
        private readonly IStatisticsService _statisticsService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApiRootController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

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
