using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo1.Jobs;
using Hangfire.Storage.Monitoring;
using HangfireDemo1.Models;


namespace HangfireDemo1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        // ---- Model Data
        public JobList<ProcessingJobDto>? CurrProcessingJobs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string JobNameString { get; set; } = "Nameless Job";

        [BindProperty(SupportsGet = true)]
        public string Param1String { get; set; } = "1";
        [BindProperty(SupportsGet = true)]
        public string JobIDString { get; set; } = "";

        public List<ProcessingJob> processingJobs { get; set; }

        // ---- Activity ---
        public IActionResult OnGet()
        {
            processingJobs = GetCurrProcessingJobs();
            return Page();
        }

        public IActionResult OnPostStopJobByID(string id)
        {
            if (id is not null)
            {
                BackgroundJob.Delete(id);
            }

            processingJobs = GetCurrProcessingJobs();
            return Page();
        }
        public IActionResult OnPostStartJob()
        {
            var jobid = BackgroundJob.Enqueue(() => StoppableJob.Execute(JobNameString, Param1String, CancellationToken.None));
            
            Console.WriteLine($"Job {JobNameString} kicked off with ID of {jobid}");
            //Settle up the model before returning.
            processingJobs = GetCurrProcessingJobs();
            return Page();
        }

        public List<ProcessingJob>? GetCurrProcessingJobs()
        {
            List<ProcessingJob> newList = new List<ProcessingJob>();

            var mon = JobStorage.Current.GetMonitoringApi();

            CurrProcessingJobs = mon.ProcessingJobs(0, int.MaxValue);
            foreach (var job in CurrProcessingJobs)
            {
                newList.Add(new ProcessingJob
                {
                    ID = job.Key,
                    Type = job.Value.Job.Type.Name,
                    Name = job.Value.Job.Args[0].ToString(),
                    Param1 = job.Value.Job.Args[1].ToString(),
                });
            }

            return newList;
        }

        public List<ProcessingJob>? GetScheduledJobs()
        {
            List<ProcessingJob> newList = new List<ProcessingJob>();

            var mon = JobStorage.Current.GetMonitoringApi();

            var ScheduledJobs = mon.ScheduledJobs(0, int.MaxValue);
            foreach (var job in ScheduledJobs)
            {
                newList.Add(new ProcessingJob
                {
                    ID = job.Key,
                    Type = job.Value.Job.Type.Name,
                    Name = job.Value.Job.Args[0].ToString(),
                    Param1 = job.Value.Job.Args[1].ToString(),
                });
            }

            if (newList.Count == 0) newList.Add(new ProcessingJob { Name = "Nothing Running" });

            return newList;
        }
    }
}
//-----------------------------------------------------------------
// Code snippets for future use...---------------------------------


//var servers = mon.Servers();
//foreach (var server in servers)
//{

//}

//var monitor = Storage.GetMonitoringApi();
//var pager = new Pager(from, perPage, monitor.ProcessingCount());
//var processingJobs = monitor.ProcessingJobs(pager.FromRecord, pager.RecordsPerPage);

//RecurringJob.AddOrUpdate(() => new MyJob().Execute(), Cron.Never);
//RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job sitting here!!"), Cron.Never);


//var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"),TimeSpan.FromDays(7));
