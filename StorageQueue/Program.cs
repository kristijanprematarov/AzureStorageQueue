using Azure.Storage.Queues;

string connectionString = "";
string queueName = "appqueue";

SendMessage("Test message 1");
SendMessage("Test message 2");

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