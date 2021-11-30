using Grpc.Core;
using GreetingProto;

namespace GreetingServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {   
        var greeting = MakeGreeting(request);
        
        return Task.FromResult(new HelloReply
        {
            Greeting = greeting
        });
    }

    private string MakeGreeting(HelloRequest request) {
        String greeting;
        switch(request.Language) {
            case "en":
                if (request.Friendliness == 1) return $"Oh, it's {request.Name}";
                if (request.Friendliness == 2) return $"Hello, {request.Name}";
                return $"Hooray! It's {request.Name}! Amazing!";
            default:
                return $"Sorry, I don't know how to greet people in {request.Language}";
        }
    }
}
