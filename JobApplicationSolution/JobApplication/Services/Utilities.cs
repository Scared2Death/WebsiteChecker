namespace JobApplication.Services
{
    public static class Utilities
    {
        public static void RaiseException(string message)
        {
            throw new Exception(message);        
        }

        public static void EnsurePathExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        public static string GetDomainFromURL(string url)
        {
            var uri = new Uri(url);
            var domain = uri.Host;

            return domain;
        }

    }
}