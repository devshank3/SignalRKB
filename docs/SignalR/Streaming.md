---
layout: page
title: "Streaming using SignalR"
permalink: /Streaming/
mermaid: true
---

ASP.NET Core SignalR enables streaming between client and server, which is beneficial for handling data that arrives in fragments over time. Instead of waiting for the complete data set, each fragment is transmitted immediately as it becomes available.

A hub method automatically becomes a streaming hub method when it returns `IAsyncEnumerable<T>`, `ChannelReader<T>`, `Task<IAsyncEnumerable<T>>`, or `Task<ChannelReader<T>>`.



