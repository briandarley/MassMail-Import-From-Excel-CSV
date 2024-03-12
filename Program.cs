// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

CreateHostBuilder(null).Build().Run();

static IHostBuilder CreateHostBuilder(string[]? args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        var env = hostContext.HostingEnvironment;
        config.AddUserSecrets<Program>();
        if (env.IsDevelopment())
        {

        }
    })
        .ConfigureServices((hostContext, services) =>
        {

            services.ConfigureServices(hostContext.Configuration);

        });