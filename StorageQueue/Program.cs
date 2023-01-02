using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "";
string queueName = "appqueue";

//SendMessage("Test message 1");
//SendMessage("Test message 2");

PeekMessages();

void PeekMessages()
{
    QueueClient queueClient = new(connectionString, queueName);
    int maxNumberOfMessagesToGetFromQueue = 10;

    PeekedMessage[] peekedMessages = queueClient.PeekMessages(10);

    Console.WriteLine("The messages in the queue are:");

    foreach (var message in peekedMessages)
    {
        Console.WriteLine(message.Body);
    }
}

void SendMessage(string message)
{
    QueueClient queueClient = new(connectionString, queueName);

    if (queueClient.Exists())
    {
        queueClient.SendMessage(message);
        Console.WriteLine("Message was sent");
    }
    else
    {
        Console.WriteLine("Failed to send message");
    }
}