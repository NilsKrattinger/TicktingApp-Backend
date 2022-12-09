namespace TicketingApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();


            //The port number(5001) must match the port of the gRPC server.
            //using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new ConcertServiceClient(channel);
            //var reply = await client.GetAllConcertsAsync(new EmptyRequest());
            //Console.WriteLine("Greeting: " + reply);
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}