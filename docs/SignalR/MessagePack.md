---
layout: page
title: ""
permalink: /MessagePack/
mermaid: true
---


MessagePack is an efficient binary serialization format. It lets you exchange data among multiple languages like JSON. But it's faster and smaller. Small integers are encoded into a single byte, and typical short strings require only one extra byte in addition to the strings themselves.

![](https://msgpack.org/images/intro.png)

Server configuration

`services.AddSignalR().AddMessagePackProtocol();`


Client configuration

```C#
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

var hubConnection = new HubConnectionBuilder()
                        .WithUrl("/chathub")
                        .AddMessagePackProtocol()
                        .Build();
```
