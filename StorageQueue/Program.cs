using System.Text;
using System.Text.Json;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using StorageQueue;

string connectionString =
    " ";
string queueName = "appqueue";

//*****************OBJECT ORIENTED*****************
SendMessage(orderId: "O1", quantity: 100);
SendMessage(orderId: "O2", quantity: 200);

//PeekMessages();

//*****************BASIC TESTS*****************
// SendMessage("Test message 1");
// SendMessage("Test message 2");

// PeekMessages();

// ReceiveMessages();

// SendMessage("Test message 1");
// SendMessage("Test message 2");
// SendMessage("Test message 3");
//
// Console.WriteLine($"The number of messages in the queue '{queueName}' is {GetQueueLength()}");

void SendMessage(string orderId, int quantity)
{
    QueueClient queueClient = new(connectionString, queueName);

    if (queueClient.Exists())
    {
        Order order = new()
        {
            OrderID = orderId,
            Quantity = quantity
        };

        var json = JsonSerializer.Serialize(order);

        byte[] bytes = Encoding.UTF8.GetBytes(json);

        var message = Convert.ToBase64String(bytes);

        queueClient.SendMessage(message);

        Console.WriteLine($"Message containing order with id {order.OrderID} has been successfully sent.");
    }
    else
    {
        Console.WriteLine("Failed to send message");
    }
}

void PeekMessages()
{
    QueueClient queueClient = new(connectionString, queueName);
    int maxNumberOfMessagesToGetFromQueue = 10;

    PeekedMessage[] peekedMessages = queueClient.PeekMessages(10);

    Console.WriteLine("The messages in the queue are:");

    foreach (var message in peekedMessages)
    {
        Order order = JsonSerializer.Deserialize<Order>(message.Body.ToString());

        Console.WriteLine($"OrderID: {order.OrderID}");
        Console.WriteLine($"Quantity: {order.Quantity}");
    }
}

int GetQueueLength()
{
    QueueClient queueClient = new(connectionString, queueName);
    
    if (!queueClient.Exists())
    {
        //NO MESSAGES IN THE QUEUE
        return 0;
    }

    QueueProperties queueProperties = queueClient.GetProperties();
    return queueProperties.ApproximateMessagesCount;
}

void ReceiveMessages()
{
    QueueClient queueClient = new(connectionString, queueName);
    int maxNumberOfMessagesToGetFromQueue = 10;

    QueueMessage[] queueMessages = queueClient.ReceiveMessages(maxNumberOfMessagesToGetFromQueue);

    foreach (var message in queueMessages)
    {
        Console.WriteLine(message.Body);
        queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
    }
}

// void PeekMessages()
// {
//     QueueClient queueClient = new(connectionString, queueName);
//     int maxNumberOfMessagesToGetFromQueue = 10;
//
//     PeekedMessage[] peekedMessages = queueClient.PeekMessages(10);
//
//     Console.WriteLine("The messages in the queue are:");
//
//     foreach (var message in peekedMessages)
//     {
//         Console.WriteLine(message.Body);
//     }
// }


// void SendMessage(string message)
// {
//     QueueClient queueClient = new(connectionString, queueName);
//
//     if (queueClient.Exists())
//     {
//         queueClient.SendMessage(message);
//         Console.WriteLine("Message was sent");
//     }
//     else
//     {
//         Console.WriteLine("Failed to send message");
//     }
// }