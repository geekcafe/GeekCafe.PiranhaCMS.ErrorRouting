using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Piranha;

namespace GeekCafe.PiranhaCMS.ErrorRouting
{
    public static class Middleware
    {
        private static IHttpClientFactory _httpClientFactory;
        public static IServiceCollection AddGeekCafeErrorRouting(this IServiceCollection services)
        {
            // register this module with piranha
            App.Modules.Register<Module>();

            return services;
        }

        /// <summary>
        /// Acutally use this module
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGeekCafeErrorRouting(this IApplicationBuilder builder, IApi api = null,
            IHttpClientFactory httpClientFactory = null)
        {
            _httpClientFactory = httpClientFactory;
            return RouteResponder(builder);
        }

        public static IApplicationBuilder UseGeekCafeErrorRoutingManagerUI(this IApplicationBuilder builder, IApi api = null)
        {
            Registration.Init(api);

            return builder;
        }


        private static IApplicationBuilder RouteResponder(IApplicationBuilder builder)
        {
            builder.Use(async (ctx, next) =>
            {
                // send the request up the chain
                try
                {
                    // go ahead and pass the current request up the chain
                    await next();

                    if (ctx.Response.StatusCode != 200 && !ctx.Response.HasStarted)
                    {
                        var x = new ErrorRouteFactory.Routes.Route(ctx.Response.StatusCode);
                        await x.Route(ctx, next, httpClientFactory: _httpClientFactory);

                        // log any non-404 errors
                        if (ctx.Response.StatusCode != 404)
                        {
                            try
                            {
                                
                                Console.WriteLine($"FAILURE: Route failed Path: {ctx?.Request?.Path} - Method: {ctx?.Request?.Method}");
                                Console.WriteLine($"FAILURE: Route Status Code: {ctx?.Response?.StatusCode} - Body: {ctx?.Response?.Body}");
                                Console.WriteLine($"FAILURE: Route failed Headers: { Newtonsoft.Json.JsonConvert.SerializeObject(ctx?.Request?.Headers)}");                               

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"FAILURE: Exception when logging the route: {ex.Message}");
                            }
                        }
                    }

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    var x = new ErrorRouteFactory.Routes.RouteException();
                    await x.Route(ctx, next, ex.Message);
                    // todo log this
                }
            });

            return builder;
        }
    }
}
