namespace HangfireDemo1.Jobs
{
    public class StoppableJob
    {
        public static void Execute(string NameText, string param1, CancellationToken cancellationToken)
        {
            var endTime = DateTime.UtcNow.AddMinutes(5);

            Console.WriteLine($"{NameText} Starting.. with concurrency of {param1}");
            try
            {
                while (endTime > DateTime.UtcNow)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Thread.Sleep(2000);
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"{NameText} politely asked to stop!.. so I'm exiting now.");
                return;
            }
            Console.WriteLine($"{NameText} finishing naturally.");
        }
    }
}
    
