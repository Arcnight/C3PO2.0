using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using System.Net;
using System.Net.Mime;
using System.Net.Http;

using Microsoft.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Serilog;

using C3PO.Utilities;

namespace C3PO.Web.Utilities
{
    public static class Http
    {
        public static IActionResult CreateGoodResponse(HttpRequest request, object payload = null)
        {
            return CreateResponse(request, HttpStatusCode.OK, payload);
        }

        public static IActionResult CreateResponse(HttpRequest request, HttpStatusCode code)
        {
            return CreateResponse(request, code, null);
        }

        public class ListObject
        {
            public IQueryable<object> Items { get; set; }
            public int Count { get; set; }
            //public string Type { get; set; }              // 2016-05-13 - CEby - Removed per CWeger & BBennett since the front end should not be using this field
        }

        public static IActionResult CreateListResponse(HttpRequest request, IEnumerable<object> payload, int? count = null)
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

            return CreateResponse(request, HttpStatusCode.OK, payload);
            //var aResponse = request.CreateResponse(HttpStatusCode.OK, returnList);
            //aResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            //return aResponse;
        }

        public static IActionResult CreateResponse(HttpRequest request, HttpStatusCode code, object payload)
        {
            var response = new ObjectResult(payload) { StatusCode = (int?)code };
            return response;

            //var aResponse = request.CreateResponse(code, payload);
            //aResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

            //return aResponse;
        }

        public static IActionResult CreateFileResponseMessageResponse(HttpRequest request, ILogger logger, byte[] attachment, string fileName, string mimeType)
        {
            ObjectResult fileResponse = new ObjectResult(null) { StatusCode = (int?)HttpStatusCode.OK };

            if (attachment == null)
            {
                logger.Error("Attachment was null in CreateFileMessageResponse");

                fileResponse.StatusCode = (int?)HttpStatusCode.InternalServerError;

                return fileResponse;
            }

            try
            {
                var file = new MemoryStream(attachment);

                fileResponse.Value = new StreamContent(file);

                try
                {
                    // Newer office files may not work with the content type so we will try & if it fails we will default to "application/octet-stream"
                    fileResponse.ContentTypes.Add(new MediaTypeHeaderValue(mimeType));
                }
                catch (FormatException ex)
                {
                    logger.Error("Error while creating media type header value (mime type).  Defaulting to application/download mime type.", ex);
                    fileResponse.ContentTypes.Add(new MediaTypeHeaderValue("application/download"));
                }

                var cd = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false /* Ask the user to download.  Maps to "attachment" */
                };

                /*
                fileResponse.Content.Headers.Add("Content-Disposition", cd.ToString());
                fileResponse.Content.Headers.Add("Content-Length", file.Length.ToString());
                fileResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);
                */
            }
            catch (Exception ex)
            {
                logger.Error("Could not build message in CreateFileMessageReponse", ex);

                fileResponse.StatusCode = (int?)HttpStatusCode.InternalServerError;

                return fileResponse;
            }

            return fileResponse;
        }

        //public static HttpResponse CreateQueryableDataResponse(HttpRequest request, object payload)
        //{
        //    HttpResponse queryableDatResponse = request.CreateResponse(HttpStatusCode.OK, payload, "text/json");

        //    queryableDatResponse.Headers.Location = new Uri(request.RequestUri, string.Empty);

        //    return queryableDatResponse;
        //}
    }
}