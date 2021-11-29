using EasyNetQ;

const string AMQP = "amqps://vkkvkgxw:TJDP2yuJisg98yXtSe2tD2A4LzPFX6RU@hippy-red-gecko.rmq3.cloudamqp.com/vkkvkgxw";
var bus = RabbitHutch.CreateBus(AMQP);
int count = 0;
while(true) {
    Console.WriteLine("Press any key to send a message");
    Console.ReadKey();
    var message = new Messages.Message($"Message #{count++}");
    bus.PubSub.Publish(message);
    Console.WriteLine($"Sent {message}");
}



