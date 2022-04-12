using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace GeekCafe.PiranhaCMS.ErrorRouting.ErrorRouteFactory.Routes
{
    public abstract class RouteBase
    {
        private string USER_AGENT = "GeekCafeErrorPageUserAgent";

        public virtual int StatusCode { get; set; }
        public virtual string ManagerRoute { get; set; }
        public virtual string PublicRoute { get; set; }
        public virtual string CatchAllMessage { get; set; }
        public RouteBase()
        {
            
        }

        public virtual async Task Route(HttpContext ctx, Func<Task> next, string message = "", IHttpClientFactory httpClientFactory = null)
        {
            // if something bad happend catch the error
            // todo send to a factory for processing
            if ((ctx.Response.StatusCode == StatusCode || StatusCode == -1) && !ctx.Response.HasStarted)
            {
                // track the original path, in case we need it later
                string originalPath = ctx.Request.Path.Value;
                ctx.Items["originalPath"] = originalPath;
                
                // todo hit the db for custom settings

                if (originalPath.StartsWith("/manager/"))
                {
                    // goto this specfic page
                    ctx.Request.Path = ManagerRoute;
                }
                else
                {

                    // this should point to a page created in the admin (manager)
                    // (not a route)
                    ctx.Request.Path = PublicRoute;

                }


                // you must clear this out or there will be issues
                // if the "id" is there it will create a bunch of problems                        
                ctx.Request.QueryString = new QueryString("");

                
                try
                {
                    // push it back into the middleware
                    // bug: this is not working as expected since upgrading to v10 of piranha
                    await next();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

                // final catch all
                if ((ctx.Response.StatusCode == StatusCode || StatusCode == -1) && !ctx.Response.HasStarted)
                {
                    var html = $"{CatchAllMessage} {message}";
                    try
                    {
                        if (IsOkToProcess(ctx))
                        {
                            // v10+ workaround, the above next() doesn't push it back it to the
                            // correct pipeline.  This will attempt to find and load page and
                            // return the raw html
                            var protocol = (ctx.Request.IsHttps) ? "https://" : "http://";
                            var url = Path.Join($"{protocol}{ctx.Request.Host}", PublicRoute);
                            html = await GetPage(httpClientFactory, url, html);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception in error routing page {ex.Message}");
                    }
                    finally
                    {
                        // either output the found html or the passed in values
                        await ctx.Response.WriteAsync(html);
                    }
                    
                    
                }
            }
        }

        public bool IsOkToProcess(HttpContext ctx)
        {

            return (ctx.Request.Headers.UserAgent != USER_AGENT);
          
        }

        public async Task<string> GetPage(IHttpClientFactory httpClientFactory , string path, string defaultHtml)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, path)
            {
                Headers =
                {
                    { HeaderNames.Accept, "html/text" },
                    { HeaderNames.UserAgent, USER_AGENT }
                }
            };

            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var reader = new StreamReader(contentStream);
                
                var html = await reader.ReadToEndAsync();

                return html;
            }

            return defaultHtml;
        }
    }
}
