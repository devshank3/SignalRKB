---
layout: page
title: ""
permalink: /ChoosingTransports/
mermaid: true
---

## Setting Transport Types in SignalR Connection Builder

When building a SignalR client connection, you can specify the transport types that the client should use. SignalR supports multiple transport types, and you can configure the client to fall back to different transport types if the preferred one is not available.


### Transport Types

- WebSockets: The most efficient transport type, providing full-duplex communication over a single, long-lived connection.
- Server-Sent Events (SSE): A unidirectional channel from the server to the client. Less efficient than WebSockets but more efficient than long polling.
- Long Polling: The least efficient transport type, where the client repeatedly polls the server for updates.


### Configuring Transport Types

You can configure the transport types in the `HubConnectionBuilder` using the `WithUrl` method:

```C#
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

HubConnection connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7142/accessorHub", options =>
    {
        options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
    })
    .Build();

```
In the above provided, the client will attempt to connect using WebSockets first. If WebSocket is not available, it will fall back to Server-Sent Events (SSE). If SSE is also not available, it will finally fall back to Long Polling.

---

Tips to fake making websocket not available in browser JS code, 

```JavaScript
import * as signalR from "@microsoft/signalr";

WebSocket = undefined;

```

---

