namespace UserRegistrationAndGameLibrary.Api;

public class ApiEntryPoint
{
    public ApiEntryPoint() { }

    public static void Main(string[] args)
    {
        // Dummy method - never actually runs
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<DummyStartup>();
            });

    private class DummyStartup
    {
        public void Configure(IApplicationBuilder app) { }
        public void ConfigureServices(IServiceCollection services) { }
    }
}
