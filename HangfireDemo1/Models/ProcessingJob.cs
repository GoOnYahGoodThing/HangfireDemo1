using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HangfireDemo1.Models
{
    public class ProcessingJob
    {
        [Display(Name = "Job ID")] 
        public string ID { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [Display(Name = "Instance")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Concurrency")]
        public string Param1 { get; set; } = string.Empty;
        
    }
}
