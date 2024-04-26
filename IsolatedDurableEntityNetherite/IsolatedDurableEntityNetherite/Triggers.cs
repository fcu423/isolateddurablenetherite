using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask.Entities;
using System.Net;

namespace IsolatedDurableEntityNetherite
{
    public class Triggers
    {
        // This trigger fails because netherite for entities isolated has not yet released:
        // https://github.com/microsoft/durabletask-netherite/issues/361#issuecomment-2005172334
        [Function("Trigger")]
        public async Task<HttpResponseData> HttpGetState(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            [DurableClient] DurableTaskClient durableEntityClient,
            FunctionContext executionContext)
        {
            var entityId = new EntityInstanceId(nameof(Counter), "myCounter1");
            var workflowEntity = await durableEntityClient.Entities.GetEntityAsync<Counter>(entityId);

            if (workflowEntity == null)
                return req.CreateResponse(HttpStatusCode.NotFound);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(workflowEntity.State.ToString() ?? "");
            return response;
        }
    }
}
