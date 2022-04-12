using System;
namespace GeekCafe.PiranhaCMS.ErrorRouting.ErrorRouteFactory.Routes
{
    public class Route: RouteBase
    {
        public Route(int code)
        {
            StatusCode = code;
            ManagerRoute = $"/manager/errors/{code}";
            PublicRoute = $"/errors/{code}";
            CatchAllMessage = "" +
                "<html>" +
                $"<head><title>{code}</title></head>" +
                $"<body>Something went wrong - {code} error is not configured for a specific page.</body>" +
                "</html>";
        }
    }
}
