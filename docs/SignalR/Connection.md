---
layout: page
title: ""
permalink: /Connection/
mermaid: true
---

### Summary of SignalR Connections and Contexts

In SignalR, a connection (or context) represents an individual connection to the server. Each connection has its own context with several important properties:

1. **ConnectionId**
   - A unique identifier for each connection.
   - It is a GUID by default but can be overridden (not recommended).
   - Refreshing the page generates a new ConnectionId.

2. **UserIdentifier**
   - Used when integrating with ASP.NET Identity.
   - Represents the username or unique identifier of the authenticated user.
   - Only available if the user is authenticated.

3. **User**
   - The entire principal object from ASP.NET Identity.
   - Provides more detailed information about the authenticated user.
   - Only available if the user is authenticated.

4. **Items**
   - A dictionary for storing metadata related to the connection.
   - Useful for persisting data across multiple hub calls.

5. **ConnectionAborted**
   - A cancellation token that indicates if the connection has been aborted.
   - Useful for handling long-running asynchronous tasks.

Understanding these properties helps in managing and utilizing SignalR connections effectively.