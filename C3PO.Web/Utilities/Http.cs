using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using System.Net;
using System.Net.Mime;

using System.Net.Http;
using System.Net.Http.Headers;

using Serilog;

using C3PO.Utilities;

namespace C3PO.Web.Utilities
{
    public static class Http
    {
        public static HttpResponseMessage CreateGoodResponse(HttpRequestMessage request, object payload = null)
        {
            return (payload == null) ?
                CreateResponse(request, HttpStatusCode.OK) :
                CreateResponse(request, HttpStatusCode.OK, payload);
        }

        public static HttpResponseMessage CreateResponse(HttpRequestMessage request, HttpStatusCode code)
        {
            return CreateResponse(request, code, null);
        }

        public class ListObject
        {
            public IQueryable<object> Items { get; set; }
            public int Count { get; set; }
            //public string Type { get; set; }              // 2016-05-13 - CEby - Removed per CWeger & BBennett since the front end should not be using this field
        }

        public static HttpResponseMessage CreateListResponse(HttpRequestMessage request, IEnumerable<object> payload, int? count = null)
        {
            ListObject returnList;
            if (payload == null)
            {
                //TODO: log something
                returnList = new ListObject();
            }
            else
            {
                returnList = new ListObject()
                {
                    Items = payload.AsQueryable(),
                    Count = count ?? payload.Count()
                };
            }
            var aResponse = request.CreateResponse(HttpStatusCode.OK, returnList);
            aResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            return aResponse;
        }

        public static HttpResponseMessage CreateResponse(HttpRequestMessage request, HttpStatusCode code, object payload)
        {
            var aResponse = request.CreateResponse(code, payload);
            aResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            return aResponse;
        }

        public static HttpResponseMessage CreateErrorResponse(HttpRequestMessage request, HttpStatusCode code, string message)
        {
            var aResponse = request.CreateErrorResponse(code, message);
            aResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            return aResponse;
        }

        public static HttpResponseMessage CreateFileResponseMessageResponse(HttpRequestMessage request, ILogger logger, byte[] attachment, string fileName, string mimeType)
        {
            HttpResponseMessage fileResponse = request.CreateResponse(HttpStatusCode.OK);

            if (attachment == null)
            {
                logger.Error("Attachment was null in CreateFileMessageResponse");
                fileResponse = request.CreateResponse(HttpStatusCode.InternalServerError);
                return fileResponse;
            }

            try
            {
                var file = new MemoryStream(attachment);

                fileResponse.Content = new StreamContent(file);

                try
                {
                    // Newer office files may not work with the content type so we will try & if it fails we will default to "application/octet-stream"
                    fileResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                }
                catch (FormatException ex)
                {
                    logger.Error("Error while creating media type header value (mime type).  Defaulting to application/download mime type.", ex);
                    fileResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/download");
                }

                var cd = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false /* Ask the user to download.  Maps to "attachment" */
                };

                fileResponse.Content.Headers.Add("Content-Disposition", cd.ToString());
                fileResponse.Content.Headers.Add("Content-Length", file.Length.ToString());
                fileResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);
            }
            catch (Exception ex)
            {
                logger.Error("Could not build message in CreateFileMessageReponse", ex);
                fileResponse = request.CreateResponse(HttpStatusCode.InternalServerError);
                return fileResponse;
            }

            return fileResponse;
        }

        public static HttpResponseMessage CreateQueryableDataResponse(HttpRequestMessage request, object payload)
        {
            HttpResponseMessage queryableDatResponse = request.CreateResponse(HttpStatusCode.OK, payload, "text/json");

            queryableDatResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            return queryableDatResponse;
        }
    }
}