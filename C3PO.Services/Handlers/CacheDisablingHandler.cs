using System;
using System.Net.Http;

using System.Threading;
using System.Threading.Tasks;

namespace C3PO.Web.Handlers
{
    internal class CacheDisablingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;

                try
                {
                    response.Headers.Add("Pragma", "no-cache");
                    response.Headers.Add("Cache-Control", "no-cache");

                    if (response.Content != null) response.Content.Headers.Expires = DateTime.Now.AddDays(-2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return task.Result;
            }, cancellationToken);
        }
    }
}