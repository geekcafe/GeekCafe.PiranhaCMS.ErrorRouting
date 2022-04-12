# GeekCafe.PiranhaCMS.ErrorRouting


Currently it expects pages to either physically exist or be present in Piranha CMS with the following paths:

- Manger Errors/Not Found
    - /manager/errors/{code}

- CMS UI Errors/Not Found
    - /errors/{code}

e.g
```
/errors/404
/errors/500
etc
```

Fallback, if it fails to find a valid path the following basic internal template is used:

```c#
CatchAllMessage = "" +
    "<html>" +
    $"<head><title>{code}</title></head>" +
    $"<body>Something went wrong - {code} error is not configured for a specific page.</body>" +
    "</html>";
```

# Adding to your project

```c#
var builder = WebApplication.CreateBuilder(args);

// attached the httpclient to DI (used in the routing)
builder.Services.AddHttpClient();
// add the routing
builder.Services.AddGeekCafeErrorRouting();

...

// use the routing and add the HttpClient service added to DI
app.UseGeekCafeErrorRouting(httpClientFactory: app.Services.GetService<IHttpClientFactory>());
```

# Create a 404 page in the Piranha CMS

1. Add a new page/folder named `errors`
2. Add a sub page named 404 so that the path/slug is: `errors/404`

attempt to goto an invalid page
eg. on mine try:
https://geekcafe.com/werwklewrkewkwe