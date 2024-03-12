using Microsoft.Extensions.Hosting;
namespace Import_Data_From_Excel.WorkTasks
{
    public class Worker : BackgroundService
    {
        private IWorkerTask _workerTask;

        public Worker(IWorkerTask workerTask)
        {
            _workerTask = workerTask;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _workerTask.ExecuteAsync(stoppingToken);
        }
    }
}