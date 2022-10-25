using System.Xml.Linq;

namespace JobApplication.Configurations
{
    public class EmailConfiguration
    {
        public EmailConfiguration()
        {
            var configFilePath = "./Configurations/EmailConfiguration.xml";
            ReadConfigurationFile(configFilePath);
        }

        public string Sender { get; private set; }
        public string Password { get; private set; }
        public List<string> Recipients { get; private set; } = new();
        public string Subject { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }

        private void ReadConfigurationFile(string configFilePath)
        {
            var doc = XDocument.Load(configFilePath);

            var valueElementName = "value";

            Sender = doc.Root.Element("sender").Value;
            Password = doc.Root.Element("password").Value;

            var recipients = doc.Root.Descendants("recipient");

            foreach (var recipient in recipients)
                Recipients.Add(recipient.Value);

            Subject = doc.Root.Element("subject").Value;

            Host = doc.Root.Element("host").Value;
            Port = int.Parse(doc.Root.Element("port").Value);
        }

    }
}