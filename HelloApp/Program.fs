open HelloLib

[<EntryPoint>]
let main argv =
    let server = Greeting.Serve ()
    let client = Greeting.MakeClient ()

    client.SayHelloAsync(new HelloNet.Api.HelloRequest(Greetee = "world")).ResponseAsync.Wait()

    server.ShutdownAsync().Wait()

    0
