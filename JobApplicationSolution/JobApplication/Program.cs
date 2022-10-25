using System;

using JobApplication.Services;

namespace JobApplication
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Logger.Log("The service to check the given websites' accessibility has started ...");

                var websiteChecker = new WebsiteChecker();
                websiteChecker.StartWebsiteChecking();
            }
            catch (Exception ex)
            {
                var message = $"Some error occurred during the website cheking procedure with the details given: {Environment.NewLine} {ex.Message}";

                Utilities.RaiseException(message);
                Logger.Log(message);
            }
            finally
            {
                Logger.Log("The service to check the given websites' accessibility has stopped ...");
            }

            Console.ReadLine();
        }

    }
}