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
			this.service = service;
			this.contentFormatter = contentFormatter;
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

        public string GetContentFormatted(long id)
        {
            var content = GetContent(id);
            var formattedContent = contentFormatter.Format(content);
            return formattedContent;
        }

        public string GetContent(long id)
        {
            if(!service.IsConnected)
            {
                service.Connect();
            }
            
            var result = service.GetContent(id);
            return result;
        }
    }
}
