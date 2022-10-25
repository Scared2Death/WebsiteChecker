using JobApplication.Interfaces;
using JobApplication.Configurations;
using System.Net;

namespace JobApplication.Services
{
    public class WebsiteChecker
    {
        private List<INotifier> notifiers = new();
        public void StartWebsiteChecking()
        {
            if (WebsiteCheckerConfiguration.IsConsoleNotificationEnabled)
                notifiers.Add(new ConsoleNotifier());

            if (WebsiteCheckerConfiguration.IsEmailNotificationEnabled)
                notifiers.Add(new EmailNotifier());

            while (true)
            {
                DoWebsiteChecking();

                Thread.Sleep(WebsiteCheckerConfiguration.TimeIntervalBetweenWebsiteChecks);
            }
        }

        private void DoWebsiteChecking()
        {
            foreach (var url in WebsiteCheckerConfiguration.WebsiteURLs)
            {
                var result = TryToAccessWebsite(url);

                if (result.isWebsiteAccessible)
                {
                    if (result.wasWebsiteAccessedInTime)
                    {
                        Logger.Log($"The website at {url} was successfully accessed in time .");
                    }
                    else
                    {
                        var message = $"The website at {url} was successfully accessed but not in time .";

                        Logger.Log(message);

                        foreach (var notifier in notifiers)
                            notifier.Notify(message);
                    }
                }
                else
                {
                    var message = $"The website at {url} was not accessible .";

                    Logger.Log(message);

                    foreach (var notifier in notifiers)
                        notifier.Notify(message);
                }
            }
        }

        private (bool isWebsiteAccessible, bool wasWebsiteAccessedInTime) TryToAccessWebsite(string url)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            try
            {
                var result = ping.Send(url);

                if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    if (result.RoundtripTime <= WebsiteCheckerConfiguration.MaxTimeForWebsiteAccess)
                    {
                        return (isWebsiteAccessible: true, wasWebsiteAccessedInTime: true);
                    }
                    else
                    {
                        return (isWebsiteAccessible: true, wasWebsiteAccessedInTime: false);
                    }
                }
                else
                {
                    return (isWebsiteAccessible: false, wasWebsiteAccessedInTime: false);
                }

            }
            catch (Exception ex)
            {
                Logger.Log($"Some error occurred while trying to connect to the following website: {url} . Details: {ex.Message}");

                return (isWebsiteAccessible: false, wasWebsiteAccessedInTime: false);
            }

        }

        // as the answers at https://stackoverflow.com/questions/7523741/how-do-you-check-if-a-website-is-online-in-c suggest there might be some better alternatives to check for a given website's presence

    }
}