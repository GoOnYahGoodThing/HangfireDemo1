namespace HangfireDemo1.Jobs
{
    public class SimpleJob
    {
        public static void Execute(string inputText, string param1)
        {
            Console.WriteLine($"Starting {inputText} at {DateTime.Now}");
            Console.WriteLine($"{inputText} param1 = {param1}");
            System.Threading.Thread.Sleep(30000);

            Console.WriteLine($"Ending {inputText} at {DateTime.Now}");
        }
    }
}
