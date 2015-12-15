using System;

using System.Net;
using System.Net.Http;

namespace C3PO.Utilities
{
    public static class Web
    {
        public static HttpResponseMessage CreateResponse<T>(HttpRequestMessage request, T content)
        {
            HttpResponseMessage response = null;

            if (request == null)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                response = (content == null) ?
                    request.CreateResponse(HttpStatusCode.OK) :
                    request.CreateResponse<T>(HttpStatusCode.OK, content, "text/json");

                response.Headers.Location = new Uri(request.RequestUri, string.Empty);
            }

            return response;
        }
    }
}
