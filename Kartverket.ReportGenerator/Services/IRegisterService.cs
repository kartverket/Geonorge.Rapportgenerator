using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Services
{
    public interface IRegisterService
    {
        Dictionary<string, string> GetFylker();
        Dictionary<string, string> GetKommuner();
    }
}