---
layout: page
title: ""
permalink: /BasicServerClient/
mermaid: true
---

# Building a Basic .NET Core Server and .NET Framework Client with SignalR

In this post, we will see the process of setting up a basic .NET Core server and a .NET Framework client (yes both are .NET) using SignalR for real-time communication. 

We will cover the server-side setup, client-side setup, and handling connection events.

## Prerequisites

- Visual Studio 2022
- .NET 8 SDK
- .NET Framework 4.8

---

### Setting Up the .NET Core Server

#### Create a New .NET Core Project
- Create a new `ASP.NET Core Web App` project, ideally you will have a basic `WeatherForecast` template with controllers. 

#### Create the SignalR Hub

Create a new class AccessorHub.cs possibly in `Hubs` folder.

```C#
using Microsoft.AspNetCore.SignalR;

public class AccessorHub : Hub
{
    public static int ViewCount { get; set; } = 0;

    public async Task NotifyWatching()
    {
        ViewCount++;

        // Notify all clients
        await Clients.All.SendAsync("UpdateViewCount", ViewCount);
    }
}

```
This hub will manage the view count and notify all connected clients whenever the view count is updated.

#### Configure the Server

Update the Program.cs file to configure SignalR and map the hub

```C#
app.MapHub<AccessorHub>("/accessorHub");
```
---

### Setting Up the .NET Framework Client

Next, let's create a .NET Framework client that will connect to the SignalR server and handle real-time updates.

- Create a New .NET Framework console app project and install SignalR Client package
    - Install through Nuget package manager `Microsoft.AspNetCore.SignalR.Client`

#### Client implementation

Update the Program.cs file to connect to the SignalR server and handle connection events

```C#
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
    static async Task Main(string[] args)
    {
        HubConnection connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7142/accessorHub")
            .Build();

        connection.Closed += async (error) =>
        {
            Console.WriteLine("Connection closed. Trying to restart...");
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };

        connection.Reconnecting += error =>
        {
            Console.WriteLine("Connection lost. Reconnecting...");
            return Task.CompletedTask;
        };

        connection.Reconnected += connectionId =>
        {
            Console.WriteLine($"Reconnected. ConnectionId: {connectionId}");
            return Task.CompletedTask;
        };

        connection.On<int>("UpdateViewCount", (viewCount) =>
        {
            Console.WriteLine($"View count: {viewCount}");
        });

        try
        {
            connection.StartAsync().Wait();
            Console.WriteLine("Connection started successfully.");
            await connection.InvokeAsync("NotifyWatching");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }

        Console.ReadLine(); // Keep the console open
    }
}
```
- **HubConnection Setup**
    - Use HubConnectionBuilder to create a new SignalR connection.
    - Specify the URL of the SignalR hub with WithUrl.
    - Build the connection with Build.

- **Event Handling:**
    - **Closed event:**
        - Triggered when the connection is closed.
        - Logs a message and attempts to restart the connection after a random delay.
    - **Reconnecting event:**
        - Triggered when the connection is lost.
        - Logs a message indicating reconnection attempts.
    - **Reconnected event:**
        - Triggered when the connection is successfully re-established.
        - Logs a message with the new connection ID.
- **Handling Incoming Messages:**
    - Subscribe to the `UpdateViewCount` event from the server using `On`.
    - Handle incoming messages by printing the view count.
- **Starting the Connection:**
    - Use `StartAsync` to start the connection asynchronously.
    - Call `InvokeAsync` to notify the server that a client is watching.
    - Include exception handling to log any connection failures.
- **Keeping the Console Open:**
    - Use `Console.ReadLine` to keep the console application open for observing connection status and messages.

---

### Running the applications 

Spin up the server and start multiple instances of the console client, You should see the client successfully connect to the server and receive updates about the view count.

--- 

So, we set up a basic .NET Core server and a .NET Framework client using SignalR for real-time communication. We covered the server-side setup, client-side setup, and handling connection events.

---