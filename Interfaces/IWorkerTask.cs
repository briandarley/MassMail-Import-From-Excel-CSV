public interface IWorkerTask
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}