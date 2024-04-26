using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Entities;
using Microsoft.Extensions.Logging;

public class Counter : TaskEntity<int>
{
    readonly ILogger logger;

    public Counter(ILogger<Counter> logger)
    {
        this.logger = logger;
    }

    public void Add(int amount) => this.State += amount;

    public void Remove(int amount) => this.State -= amount;

    public void Reset() => this.State = 0;

    public int Get() => this.State;

    [Function(nameof(Counter))]
    public static Task Run([EntityTrigger] TaskEntityDispatcher dispatcher)
         => dispatcher.DispatchAsync<Counter>();
}