namespace EAppointment.Application.Saga;

public interface ISaga<TData>
{
    Task ExecuteAsync(TData data, CancellationToken cancellationToken = default);
    Task CompensateAsync(TData data, CancellationToken cancellationToken = default);
}

public abstract class SagaStep<TData>
{
    public abstract Task ExecuteAsync(TData data, CancellationToken cancellationToken = default);
    public abstract Task CompensateAsync(TData data, CancellationToken cancellationToken = default);
}

public class SagaOrchestrator<TData>
{
    private readonly List<SagaStep<TData>> _steps = new();
    private readonly Stack<SagaStep<TData>> _executedSteps = new();

    public void AddStep(SagaStep<TData> step)
    {
        _steps.Add(step);
    }

    public async Task<bool> ExecuteAsync(TData data, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var step in _steps)
            {
                await step.ExecuteAsync(data, cancellationToken);
                _executedSteps.Push(step);
            }
            return true;
        }
        catch (Exception)
        {
            await CompensateAsync(data, cancellationToken);
            return false;
        }
    }

    private async Task CompensateAsync(TData data, CancellationToken cancellationToken = default)
    {
        while (_executedSteps.Count > 0)
        {
            var step = _executedSteps.Pop();
            try
            {
                await step.CompensateAsync(data, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log compensation failure
                Console.WriteLine($"Compensation failed for step {step.GetType().Name}: {ex.Message}");
            }
        }
    }
}
