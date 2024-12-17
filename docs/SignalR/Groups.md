---
layout: page
title: ""
permalink: /Groups/
mermaid: true
---

## Groups in SignalR Hubs

While building larger applications using SignalR, the need to track connection IDs for individual connections and logically grouping them as per business needs is a must. However, instead of managing connection IDs manually, SignalR provides a powerful feature called Groups. Groups allows to manage connections more efficiently and send messages to multiple clients simultaneously.

### What are SignalR Groups?

SignalR Groups are logical collections of connections. A single connection can belong to multiple groups, and groups can be dynamically created and managed. This feature is particularly useful for scenarios where you need to broadcast messages to specific subsets of connected clients.

### Example Scenario: ColorHub and ColorConsole

Let's look into an example using a simple server (ColorHub.cs) and a console client (Program.cs). In this example, clients can join color groups (Red, Blue, Green) and trigger background color changes for all clients in a specific group.

The `ColorHub` class defines two methods: `JoinGroup` and `TriggerGroup`. (This is from Kevin Griffin's SignalR Mastery course)

```C#
public class ColorHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task TriggerGroup(string groupName)
    {
        return Clients.Group(groupName).SendAsync("TriggerColor", groupName);
    }
}
```
- JoinGroup: Adds the current connection to a specified group.
- TriggerGroup: Sends a message to all clients in the specified group.

The `Program` file in the `ColorConsole` demonstrates how to interact with the `ColorHub`.

We set up a handler to listen for the TriggerColor event, triggered through Group name, (Broadcasting only to that group)

```C#
connection.On<string>("TriggerColor", groupName =>
{
    switch (groupName)
    {
        case "Red":
            Console.BackgroundColor = ConsoleColor.Red;
            break;
        case "Blue":
            Console.BackgroundColor = ConsoleColor.Blue;
            break;
        case "Green":
            Console.BackgroundColor = ConsoleColor.Green;
            break;
        default:
            Console.BackgroundColor = ConsoleColor.Black;
            break;
    }

    Console.Clear();
    Console.WriteLine($"Background color changed for group: {groupName}");
});
```
Joining and Triggering Groups, We provide options for the console user to join a group or trigger a color change.

```C#
while (true)
{
    Console.WriteLine("Enter '1' to Join a Group, '2' to Trigger background color, or 'e' to exit:");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.WriteLine("Enter group name to join (Red, Blue, Green):");
            var groupName = Console.ReadLine();
            if (IsValidGroupName(groupName))
            {
                await JoinGroup(connection, groupName);
            }
            else
            {
                Console.WriteLine("Invalid group name. Please enter Red, Blue, or Green.");
            }
            break;

        case "2":
            Console.WriteLine("Enter group name to trigger color (Red, Blue, Green):");
            var triggerGroupName = Console.ReadLine();
            if (IsValidGroupName(triggerGroupName))
            {
                await TriggerGroup(connection, triggerGroupName);
            }
            else
            {
                Console.WriteLine("Invalid group name. Please enter Red, Blue, or Green.");
            }
            break;

        case "e":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine("Invalid input. Please try again.");
            break;
    }
}
```

 Groups simplifies the management of connections and allows you to broadcast messages to specific subsets of clients efficiently. The above snippets explains how to join groups and trigger events for those groups using a simple server and console client. This approach can be extended to more complex scenarios, and it is also possible for a connection to be in multiple groups.

---