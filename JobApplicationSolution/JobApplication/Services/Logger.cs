using JobApplication.Configurations;

namespace JobApplication.Services
{
    public static class Logger
    {
        private static readonly string dateTimeFormat = "yyyy-MM-dd h:mm tt";
        private static string currentDateTime => DateTime.Now.ToString(dateTimeFormat);

        static Logger()
        {
            var directoryPath = Path.GetDirectoryName(WebsiteCheckerConfiguration.LogFileNameFQ);
            Utilities.EnsurePathExists(directoryPath);
        }

        public static void Log(string msg)
        {
            var message = $"[{currentDateTime}]: {msg}";

            if (WebsiteCheckerConfiguration.IsLoggingToConsole)
                Console.WriteLine(message);

            if (WebsiteCheckerConfiguration.IsLoggingToFile)
            {
                if (WebsiteCheckerConfiguration.IsLoggingToFileWithOverriding)
                {
                    using var streamWriter = new StreamWriter(WebsiteCheckerConfiguration.LogFileNameFQ, append: false);

                    streamWriter.WriteLine(message);
                }
                else
                {
                    using var streamWriter = new StreamWriter(WebsiteCheckerConfiguration.LogFileNameFQ, append: true);

                    streamWriter.WriteLine(message);
                }
            }
        }
   
    }
}