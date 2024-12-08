---
layout: page
title: ""
permalink: /TransportTypes/
mermaid: true
---

# Supported transport types 

SignalR is an abstraction over some of the transports that are required to do real-time work between client and server

HTML 5 transports depend on browser support. If unsupported, older transports are used. WebSocket provides a persistent, two-way connection but requires modern browsers. Server Sent Events (EventSource) are supported by most browsers except Internet Explorer. Comet transports include Forever Frame (Internet Explorer only) and Ajax long polling, which maintains a long-held HTTP request for server push data.

1. WebSockets
2. SSE (Server sent events)
3. Long Polling 

SignalR by default uses WebSocket transport and it falls back to older transports mechanisms when WebSocket is not supported or if older transports are preferred.


### Traditional HTTP

We know about basic HTTP request response model. 

```mermaid
sequenceDiagram
    participant Client
    participant Server

    Client->>Server: Request
    Server-->>Client: Response
```
---

### Websockets 

WebSocket establishes persistent, two way connection between client and server. 

```mermaid

    flowchart TD
    n1["Server"] <-- "<span style=color:>WebSocket&nbsp;</span>" --> n2["Client 1"]
    n1 <-- "<span style=background-color:>WebSocket</span><span style=background-color:>&nbsp;</span>" --> n4["Client 2"] & n3["Client 3"]

    n1@{ shape: rect}
    n2@{ shape: rect}
    n4@{ shape: rect}
    n3@{ shape: rect}


```

---


SSE - Server-sent events 

    Also known as EventSource

    Forever Frames

Long polling 


---
Transportation selection steps followed by signalR 

A flowchart to support explaining this selection process. 

```mermaid
graph TD
    A[V] --> B{Is browser IE8 or earlier?}
    B -- Yes --> C[Use Long Polling]
    B -- No --> D{Is JSONP configured?}
    D -- Yes --> C
    D -- No --> E{Is connection cross-domain?}
    E -- No --> F{Do client and server support WebSocket?}
    F -- Yes --> G[Use WebSocket]
    F -- No --> H{Is Server Sent Events available?}
    H -- Yes --> I[Use Server Sent Events]
    H -- No --> J{Is Forever Frame available?}
    J -- Yes --> K[Use Forever Frame]
    J -- No --> C
    E -- Yes --> L{Do client and server support CORS and WebSocket?}
    L -- Yes --> G
    L -- No --> C
```


---