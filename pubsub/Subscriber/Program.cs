using EasyNetQ;

const string AMQP = "amqps://vkkvkgxw:TJDP2yuJisg98yXtSe2tD2A4LzPFX6RU@hippy-red-gecko.rmq3.cloudamqp.com/vkkvkgxw";
var bus = RabbitHutch.CreateBus(AMQP);

var subscriberId = Guid.NewGuid().ToString();

bus.PubSub.Subscribe<string>(subscriberId, message => {
    Console.WriteLine($"[string] Message: {message}");
});

bus.PubSub.Subscribe<Messages.Message>(subscriberId, message => {
    Console.WriteLine("RECEIVED A STRONGLY TYPED MESSAGE!");
    Console.WriteLine(message);
});


Console.WriteLine("Listening for messages! Press Enter to quit.");
Console.ReadLine();