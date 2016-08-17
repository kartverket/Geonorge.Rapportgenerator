using Kartverket.ReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Services
{
    public class ReportService: IReportService
    {
        private readonly ReportDbContext _dbContext;

        public ReportService(ReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}