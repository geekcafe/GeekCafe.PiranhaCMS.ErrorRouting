using System;
namespace GeekCafe.PiranhaCMS.ErrorRouting.ErrorRouteFactory.Routes
{
    public class RouteException : RouteBase
    {
        public RouteException()
        {
            StatusCode = -1;
            ManagerRoute = "/manager/errors/500";
            PublicRoute = "/errors/500";
            CatchAllMessage = "Something went wrong - 500 error";
        }
    }
}
