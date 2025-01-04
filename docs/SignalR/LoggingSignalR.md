---
layout: page
title: ""
permalink: /Logging/
mermaid: true
---

# Logging for SignalR diagnostics in Server and Client side

## Server-side logging

To view the logs in the server console, we can edit the `appsettings.json` or configure in `CreateWebHostBuilder` method. 

Add these to `appsettings.json`
```
"Microsoft.AspNetCore.SignalR": "Debug",
"Microsoft.AspNetCore.Http.Connections": "Debug"
```

Or in the `program.cs` we can configure the `builder`

```
// Configure logging
builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
```
---

## Client side logging 

We should add `Microsoft.Extensions.Logging.Console` package explicitly for console logging or install for any other logging providers. 

During the initiation of `HubConnectionBuilder` configure the logs.
```C#
var connection = new HubConnectionBuilder()
    .WithUrl("https://url.com/hub/url")
    .ConfigureLogging(logging =>
    {
        // Log to the Console
        logging.AddConsole();

        // This will set ALL logging to Debug level
        logging.SetMinimumLevel(LogLevel.Debug);
    })
    .Build();
```
---