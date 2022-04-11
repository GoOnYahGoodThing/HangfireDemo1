namespace HangfireDemo1.Jobs
{
    public class StoppableJob
    {
        public static void Execute(string NameText, string param1, CancellationToken cancellationToken, Hangfire.Server.PerformContext context)
        {
            var endTime = DateTime.UtcNow.AddMinutes(5);
            var currentJobId = context.BackgroundJob.Id;

            Console.WriteLine($"JOB#{currentJobId}: {NameText} Starting.. with concurrency of {param1}");
            try
            {
                while (endTime > DateTime.UtcNow)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(2000);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"JOB#{currentJobId}: {NameText} politely asked to stop!.. so I'm exiting now.");
                return;
            }
            Console.WriteLine($"JOB#{currentJobId}: {NameText} finishing naturally.");
        }
    }
}

// Got the PerformContext bit from https://discuss.hangfire.io/t/how-do-i-cancel-an-already-runnign-recurring-job/7850/2