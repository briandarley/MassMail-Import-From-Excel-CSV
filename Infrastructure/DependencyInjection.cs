using Import_Data_From_Excel.Interfaces;
using Import_Data_From_Excel.Services;
using Import_Data_From_Excel.WorkTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

public static class DependencyInjection
{

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        services.AddHttpClient();
        ConfigurHttpClient(services, configuration);
        services.AddSingleton<ITokenService, TokenService>();
        services.AddTransient<BearerTokenHandler>();

        services.AddTransient<ICsvMappingService, CsvMappingService>();
        services.AddTransient<ICsvFileProcessorService, CsvFileProcessorService>();
        services.AddTransient<IMassMailDataService, MassMailDataService>();

        services.AddTransient<IExcelFileProcessorService, ExcelFileProcessorService>();
        services.AddTransient<IExcelMappingService, ExcelMappingService>();
        services.AddSingleton<IWorkerTask, WorkerTask>();
        services.AddHostedService<Worker>();


    }

    private static void ConfigurHttpClient(IServiceCollection services, IConfiguration configuration)
    {


        var idpConfigurations = configuration.GetSection("IdpConfigurations").Get<List<IdpConfiguration>>();
        if (idpConfigurations is null)
        {
            Console.WriteLine("IdpConfigurations is null");
            throw new ArgumentNullException("IdpConfigurations is null");
        }
        //Retrieve configuration from user secrets
        idpConfigurations.Single(c => c.Name == "LOCAL_IDP").ClientId = configuration["LOCAL_IDP_CLIENT_ID"];
        idpConfigurations.Single(c => c.Name == "LOCAL_IDP").ClientSecret = configuration["LOCAL_IDP_CLIENT_SECRET"];

        idpConfigurations.Single(c => c.Name == "UAT_IDP").ClientId = configuration["UAT_IDP:CLIENT_ID"];
        idpConfigurations.Single(c => c.Name == "UAT_IDP").ClientSecret = configuration["UAT_IDP:CLIENT_SECRET"];



        services.AddHttpClient("LOCAL_AD", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5503/v1/");
            client.Timeout = new TimeSpan(0, 1, 0);
        })
        .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("LOCAL_IDP", idpConfigurations)))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        services.AddHttpClient("LOCAL_DATA", client =>
        {
            client.BaseAddress = new Uri("https://localhost:15501/v1/");
            client.Timeout = new TimeSpan(0, 1, 0);
        })
        .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("LOCAL_IDP", idpConfigurations)))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        services.AddHttpClient("UAT_DATA", client =>
         {
             client.BaseAddress = new Uri("https://its-idmuat-web.ad.unc.edu/services/data.api/v1/");
             client.Timeout = new TimeSpan(0, 1, 0);
         })
         .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("UAT_IDP", idpConfigurations)))
         .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
         {
             ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
         });

        services.AddHttpClient("UAT_MASSMAIL_DATA", client =>
         {
             client.BaseAddress = new Uri("https://its-idmuat-web.ad.unc.edu/services/dal.massmail/v1/");
             client.Timeout = new TimeSpan(0, 1, 0);
         })
         .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("UAT_IDP", idpConfigurations)))
         .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
         {
             ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
         });

    }





}