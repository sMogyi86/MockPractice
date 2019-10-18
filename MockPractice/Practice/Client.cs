using System;

namespace MockPractice
{
    public class Client : IDisposable
    {
        readonly IContentFormatter contentFormatter;
        readonly IService service;

        readonly int identity = 2;

        public Client(IService service, IContentFormatter contentFormatter)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.contentFormatter = contentFormatter ?? throw new ArgumentNullException(nameof(contentFormatter));
        }

        public string GetIdentity()
        {
            return identity.ToString();
        }

        public string GetIdentityFormatted()
        {
            return $"<formatted> {identity} </formatted>";
        }

        public string GetServiceName()
        {
            return service.Name;
        }

        public void Dispose()
        {
            service.Dispose();
        }

        public string GetContent(long id)
        {
            if (!service.IsConnected)
            {
                service.Connect();
            }

            var result = service.GetContent(id);
            return result;
        }

        public string GetContentFormatted(long id)
        {
            var content = GetContent(id);
            var formattedContent = contentFormatter.Format(content);
            return formattedContent;
        }
    }
}