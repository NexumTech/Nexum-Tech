namespace NexumTech.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args)
                .UseStartup<Startup>();
        }
    }
}