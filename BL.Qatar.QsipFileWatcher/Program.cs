using BL.Internal.Messaging;
using BL.Internal.Messaging.Interfaces;
using BL.Qatar.QsipFileWatcher.InternalEvents.IntegrationEventHandlers;
using BL.Qatar.QsipFileWatcher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var titleString = "\r\n .----------------.  .----------------.  .----------------.  .----------------. \r\n| .--------------. || .--------------. || .--------------. || .--------------. |\r\n| |    ___       | || |    _______   | || |     _____    | || |   ______     | |\r\n| |  .'   '.     | || |   /  ___  |  | || |    |_   _|   | || |  |_   __ \\   | |\r\n| | /  .-.  \\    | || |  |  (__ \\_|  | || |      | |     | || |    | |__) |  | |\r\n| | | |   | |    | || |   '.___`-.   | || |      | |     | || |    |  ___/   | |\r\n| | \\  `-'  \\_   | || |  |`\\____) |  | || |     _| |_    | || |   _| |_      | |\r\n| |  `.___.\\__|  | || |  |_______.'  | || |    |_____|   | || |  |_____|     | |\r\n| |              | || |              | || |              | || |              | |\r\n| '--------------' || '--------------' || '--------------' || '--------------' |\r\n '----------------'  '----------------'  '----------------'  '----------------' \r\n";
Console.WriteLine(titleString);

var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

// Set up dependency injection
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(ValidateAndRenameSubmissionEventHandler));
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateMetsEventHandler));

    //cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    //cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    //cfg.AddOpenBehavior(typeof(TransactionBehaviour<,>));
});

builder.Services.AddSingleton<IInMemoryMessageQueue, InMemoryMessageQueue>();
builder.Services.AddSingleton<IInMemoryEventBus, InMemoryEventBus>();
builder.Services.AddSingleton<FileWatcherService>();
builder.Services.AddHostedService<InternalEventProcessorJob>();
builder.Services.AddTransient<IQatarMetsBuilder, QatarMetsBuilder>();
builder.Services.AddTransient<IArkMinterService, ArkMinterService>();

using IHost host = builder.Build();

// Start the FileWatcherService
var fileWatcher = host.Services.GetRequiredService<FileWatcherService>();
fileWatcher.StartWatching(config);

await host.RunAsync();

