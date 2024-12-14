---
layout: page
title: ""
permalink: /CreatingHubMethods/
mermaid: true
---

## Creating and Calling Hub methods from Clients 


We will see how to call hub methods from clients using SignalR. We will cover the differences between `Invoke` and `Send` methods.


### Invoke 


The `Invoke` method is used when you need to call a hub method that returns a value. This is useful when you expect a response from the server, such as fetching data or performing a calculation.

- Synchronous-like: It sends a message to the hub and waits for a response
- Promise-based: It opens a promise that resolves when the server acknowledges the method execution and returns a value
- Error Handling: If the server method throws an exception, the promise is rejected, and you can handle the error in a catch block

#### Example 

Let's say we have `GetFullName` hub method that concatenates two strings

```C#
public string GetFullName(string firstName, string lastName)
{
    return $"{firstName} {lastName}";
}
```

From the client we can call the above method using `Invoke`

```C#
try
{
    Console.WriteLine("Enter first name:");
    var firstName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    var lastName = Console.ReadLine();
    var fullName = await connection.InvokeAsync<string>("GetFullName", firstName, lastName);
    Console.WriteLine($"Full name from server: {fullName}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error calling GetFullName: {ex.Message}");
}
```

The client waits for the response and then prints the full name

---

### Send 

Usually used when you need to call a hub method that does not return a value. This is useful for notifying the server of an event or performing an action that does not require a response.

- Fire-and-forget: Send sends a message to the hub but does not wait for a response.
- Promise-based: It returns a promise that resolves when the request is sent successfully, but it does not wait for the server to acknowledge the method execution.
- No Return Value: Since it does not wait for a response, testing for a return value will yield no result or undefined.

#### Example

 A hub method `NotifyWatching` that increments a view count and notifies all connected clients

 ```C#
public async Task NotifyWatching()
{
    ViewCount++;
    await Clients.All.SendAsync("UpdateViewCount", ViewCount);
}
 ```

call this method from the client without expecting a return value, you use `Send`

```C#
try
{
    await connection.SendAsync("NotifyWatching");
}
catch (Exception ex)
{
    Console.WriteLine($"Error calling NotifyWatching: {ex.Message}");
}
```

By understanding these differences, you can choose the appropriate method based on whether you need a response from the server or not. This ensures efficient communication between your client and server using SignalR.

---