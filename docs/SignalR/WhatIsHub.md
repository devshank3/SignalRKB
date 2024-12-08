---
layout: page
title: What is SignalR Hub ?
permalink: /Hubs/
mermaid: true
---


## Hubs in ASP.NET Core SignalR

Hubs are a central concept in SignalR. They are used to manage connections, groups, and messaging between clients and servers in real-time applications. Hubs provide a high-level API that allows the server to call methods on connected clients and vice versa.

### Features of Hubs:
- **Real-time Communication:** Hubs facilitate real-time communication between the server and clients.
- **Client Method Invocation:** The server can call methods on connected clients.
- **Group Management:** Hubs can manage groups of connections, allowing messages to be sent to specific groups of clients.
- **Connection Management:** Hubs can handle connection events such as connecting, disconnecting, and reconnection.

Hubs are created by inheriting from `Hub` base class and configured in any ASP.NET application. 

```C#
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
```


### Differences Between Hubs and Controllers

| Aspect               | Hubs                                                                 | Controllers                                                   |
|----------------------|----------------------------------------------------------------------|---------------------------------------------------------------|
| **Purpose**          | Designed for real-time communication and managing connections between clients and servers. | Designed for handling HTTP requests and responses, typically for RESTful APIs. |
| **Communication**    | Enable bidirectional communication between clients and servers.      | Handle unidirectional HTTP requests from clients to servers.  |
| **State Management** | Can maintain connection state and manage groups of connections.      | Typically stateless, handling each HTTP request independently.|
| **Method Invocation**| Server can call methods on connected clients.                        | Clients call action methods on the server via HTTP requests.  |
| **Use Cases**        | Suitable for applications requiring real-time updates, such as chat applications, live notifications, and gaming. | Suitable for traditional web applications and APIs that follow the request-response model. |


---

Let us discuss more on client server hub communication later.....


Questions which can arise

- When should we use MVC controller for some actions and when should we opt for Hubs ?
    - The balance of communication 


SignalR hubs are single point of connection, we can have multiple hubs 

Not all the clients need to connect to all clients, the grouping comes into picture. 

