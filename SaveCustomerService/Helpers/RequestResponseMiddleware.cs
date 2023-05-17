using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveCustomerService.Presentation.Api.Helpers
{
    public class RequestResponseMiddleware
    {



        private readonly RequestDelegate next;
        private readonly ILogger<RequestResponseMiddleware> logger;

        public RequestResponseMiddleware(RequestDelegate Next, ILogger<RequestResponseMiddleware> Logger)
        {
            next = Next;
            logger = Logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var originalBodyStream = httpContext.Response.Body;

            var request = await GetRequestAsTextAsync(httpContext.Request);



            await using var responseBody = new MemoryStream();
          //  httpContext.Response.Body = responseBody;

            await next(httpContext);

            var response = await GetResposeAsTextAsync(httpContext.Response);


            await responseBody.CopyToAsync(originalBodyStream);


        }

        private async Task<string> GetResposeAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private async Task<string> GetRequestAsTextAsync(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = body;

            return $"{request.Scheme} {request.Host} {request.Path} {request.QueryString} {bodyAsText}";
        }
    }
}