using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Grpc.Core;
using KeyStoreService.Data;
using Newtonsoft.Json;
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
                        var req = JsonConvert.DeserializeObject<KeyRequest>(input.InputData);
                        response.OutputData = JsonConvert.SerializeObject(new { Key = Fetch(req) });
                        client.SubmitResult(response);
                        break;
                }
            }

        }

        static string Fetch(KeyRequest req)
        {
            using (var db = new AppDbContext())
                return (from k in db.KeyInfos where k.Alias == req.Alias select k.Key).SingleOrDefault();
        }

    }
}
