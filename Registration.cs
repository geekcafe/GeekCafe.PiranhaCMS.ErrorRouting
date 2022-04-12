using System;
using Piranha;
using Piranha.Manager;

namespace GeekCafe.PiranhaCMS.ErrorRouting
{
    public class Registration
    {
        public Registration()
        {
        }

        public static void Init(IApi api)
        {
            try
            {
                // add the menu item / group
                Menu.Items.Insert(2, new MenuItem
                {
                    InternalId = "GCErrorRouteModule",
                    Name = "Error Routing",
                    Css = "fas fa-exclamation-circle"

                    
                });

                // add an item list                
                Menu.Items["GCErrorRouteModule"].Items.Add(new MenuItem
                {
                    InternalId = "ErrorRoutingManagment",
                    Name = "Error Pages",
                    Route = "~/areas/manager/errorpagerouting",
                    Css = "far fa-times-circle",
                    //Policy = "MyCustomPolicy" // <!-- causes a major blow up if it doesn't exits
                });

                

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Crap something went wrong!!{ex.Message}");
            }

        }
    }
}
