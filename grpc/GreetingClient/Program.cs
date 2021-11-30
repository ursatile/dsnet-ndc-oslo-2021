using Grpc.Net.Client;
using GreetingProto;

// See https://aka.ms/new-console-template for more information
using var channel = GrpcChannel.ForAddress("https://localhost:7278");
var client = new Greeter.GreeterClient(channel);
Console.WriteLine("Connected to gRPC Server!");
while(true) {
    Console.WriteLine("Your name?");
    var name = Console.ReadLine();
    Console.WriteLine("Language?");
    var language = Console.ReadLine();
    Console.WriteLine("How friendly? (1-3)");
    var friendliness = Int32.Parse(Console.ReadLine());
    var request = new HelloRequest {
        Name = name,
        Language = language,
        Friendliness = friendliness
    };
    var reply = client.SayHello(request);
    Console.WriteLine(reply);
    Console.ReadKey();
}