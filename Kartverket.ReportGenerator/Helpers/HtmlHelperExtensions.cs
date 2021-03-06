﻿using Kartverket.ReportGenerator.Models.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Kartverket.ReportGenerator.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string ApplicationVersionNumber(this HtmlHelper helper)
        {
            string versionNumber = WebConfigurationManager.AppSettings["BuildVersionNumber"];
            return versionNumber;
        }
        public static string GeonorgeUrl(this HtmlHelper helper)
        {
            var url = WebConfigurationManager.AppSettings["GeonorgeUrl"];
            var culture = CultureHelper.GetCurrentCulture();
            if (culture != Culture.NorwegianCode)
                url = url + Culture.EnglishCode;

            return url;
        }
        public static string GeonorgeArtiklerUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["GeonorgeArtiklerUrl"];
        }
        public static string NorgeskartUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["NorgeskartUrl"];
        }
        public static string SecureNorgeskartUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["SecureNorgeskartUrl"];
        }
        public static string RegistryUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["RegistryUrl"];
        }
        public static string ObjektkatalogUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["ObjektkatalogUrl"];
        }
        public static bool DownloadServiceEnabled(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["DownloadServiceEnabled"] == "false" ? false : true;
        }
        public static string WebmasterEmail(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["WebmasterEmail"];
        }
        public static string EnvironmentName(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["EnvironmentName"];
        }

        public static string KartkatalogenUrl(this HtmlHelper helper)
        {
            return WebConfigurationManager.AppSettings["KartkatalogenUrl"];
        }

        public static bool SupportsMultiCulture(this HtmlHelper helper)
        {
            return Boolean.Parse(WebConfigurationManager.AppSettings["SupportsMultiCulture"]); ;
        }
    }
}