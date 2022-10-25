using System.Xml.Linq;

using JobApplication.Services;

namespace JobApplication.Configurations
{
    public static class WebsiteCheckerConfiguration
    {
        private static readonly string delimiter = ";";
        static WebsiteCheckerConfiguration()
        {
            var configFilePath = "./Configurations/WebsiteCheckerConfiguration.xml";
            ReadConfigurationFile(configFilePath);
        }

        private static string WebsitesToCheckFilePath { get; set; }
        private static string WebsitesToCheckFileName { get; set; }
        public static List<string> WebsiteURLs { get; private set; } = new();

        public static bool IsLoggingToConsole { get; private set; }

        public static bool IsLoggingToFile { get; private set; }
        private static string LogFilePath { get; set; }
        private static string LogFileName { get; set; }
        public static string LogFileNameFQ { get; private set; }
        public static bool IsLoggingToFileWithOverriding { get; private set; }

        public static int MaxTimeForWebsiteAccess { get; private set; }

        public static int TimeIntervalBetweenWebsiteChecks { get; private set; }

        public static bool IsConsoleNotificationEnabled { get; private set; }
        public static bool IsEmailNotificationEnabled { get; private set; }

        private static void ReadConfigurationFile(string configFilePath)
        {
            var doc = XDocument.Load(configFilePath);

            var valueElementName = "value";

            WebsitesToCheckFilePath = doc.Root.Element("websites_to_check_file_path").Element(valueElementName).Value;
            WebsitesToCheckFileName = doc.Root.Element("websites_to_check_file_name").Element(valueElementName).Value;

            ReadWebsiteURLs();

            IsLoggingToConsole = bool.Parse(doc.Root.Element("log_to_console").Element(valueElementName).Value);

            IsLoggingToFile = bool.Parse(doc.Root.Element("log_to_file").Element(valueElementName).Value);
            LogFilePath = doc.Root.Element("log_to_file").Element("file_path").Value;
            LogFileName = doc.Root.Element("log_to_file").Element("file_name").Value;
            LogFileNameFQ = Path.Combine(LogFilePath, LogFileName);
            IsLoggingToFileWithOverriding = bool.Parse(doc.Root.Element("log_to_file").Element("override").Value);

            MaxTimeForWebsiteAccess = int.Parse(doc.Root.Element("max_time_for_website_access_in_ms").Element(valueElementName).Value);

            TimeIntervalBetweenWebsiteChecks = int.Parse(doc.Root.Element("time_interval_between_website_checks_in_ms").Element(valueElementName).Value);

            IsConsoleNotificationEnabled = bool.Parse(doc.Root.Element("console_notification").Element(valueElementName).Value);
            IsEmailNotificationEnabled = bool.Parse(doc.Root.Element("email_notification").Element(valueElementName).Value);
        }

        private static void ReadWebsiteURLs()
        {
            var websitesToCheckFileNameFQ = Path.Combine(WebsitesToCheckFilePath, WebsitesToCheckFileName);
            var urlsCSVString = File.ReadAllLines(websitesToCheckFileNameFQ);

            if (urlsCSVString == null)
                Utilities.RaiseException("Website URLs to check are not correctly provided . Please check the input data .");

            foreach (var urlsLine in urlsCSVString)
            {
                var urls = urlsLine.Split(delimiter);

                foreach (var url in urls)
                {
                    var domain = Utilities.GetDomainFromURL(url);
                    WebsiteURLs.Add(domain);
                }
            }
        }

    }
}