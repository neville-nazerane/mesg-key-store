using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Grpc.Core;
using KeyStoreService.Data;
using MyTask = System.Threading.Tasks.Task;

namespace KeyStoreService.Mesg
{
    public static class MesgProvider
    {
        
        public static async MyTask ExecuteAsync()
        {

            var channel = new Channel("localhost:50052", ChannelCredentials.Insecure);
            var client = new Service.ServiceClient(channel);

            var call = client.ListenTask(new ListenTaskRequest { Token = Environment.GetEnvironmentVariable("MESG_TOKEN") }).ResponseStream;
            
            while (await call.MoveNext())
            {
                var input = call.Current;

                var response = new SubmitResultRequest {
                    OutputKey = input.TaskKey,
                    ExecutionID = input.ExecutionID
                };

                switch (input.TaskKey)
                {
                    case "fetch":
                        response.OutputData = Fetch(input.InputData);
                        client.SubmitResult(response);
                        break;
                }
            }

        }

        static string Fetch(string alias)
        {
            using (var db = new AppDbContext())
                return (from k in db.KeyInfos where k.Alias == alias select k.Key).SingleOrDefault();
            
        }

    }
}
