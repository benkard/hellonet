namespace HelloLib

open System.Threading.Tasks
open System
open Hopac
open Google.Protobuf
open Grpc.Core

module Greeting =
    let private Hello name =
        printfn "Hello %s from F#!" name

    type private HelloSayerImpl () =
        inherit HelloNet.Api.HelloSayer.HelloSayerBase()
        
        override this.SayHello(request: HelloNet.Api.HelloRequest, context : ServerCallContext) : Task<HelloNet.Api.HelloResponse> =
            Hello request.Greetee
            new HelloNet.Api.HelloResponse() |> Hopac.Job.result |> Hopac.queueAsTask
     
    let Serve () =
        let port = 10000
        let server = new Server()
        
        server.Services.Add(HelloNet.Api.HelloSayer.BindService(new HelloSayerImpl()))
        server.Ports.Add(new ServerPort("localhost", port, ServerCredentials.Insecure)) |> ignore
        server.Start()

        server
    
    let MakeClient () =
        let channel = new Channel("127.0.0.1:10000", ChannelCredentials.Insecure)
        new HelloNet.Api.HelloSayer.HelloSayerClient(channel)
