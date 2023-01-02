using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString =
    "";
string queueName = "appqueue";

// SendMessage("Test message 1");
// SendMessage("Test message 2");

// PeekMessages();

// ReceiveMessages();

SendMessage("Test message 1");
SendMessage("Test message 2");
SendMessage("Test message 3");

Console.WriteLine($"The number of messages in the queue '{queueName}' is {GetQueueLength()}");

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