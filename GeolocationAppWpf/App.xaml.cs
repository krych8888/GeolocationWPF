using DataService.Data;
using DataService.Repository;
using DataService.Repository.Interfaces;
using GeolocationAppWpf.Models;
using GeolocationAppWpf.ViewModels;
using IpStack.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using System.IO;
using System.Windows;

namespace GeolocationAppWpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IConfiguration Configuration { get; private set; }
    private readonly IHost _host;

    public App()
    {
        var builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {           
            services.AddIpStack(Configuration.GetConnectionString("IpStackAccessKeys"));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(Configuration.GetConnectionString("LocalDb")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IGeolocationService, GeolocationService>();
            services.AddTransient<GeolocationFinderViewModel>();
            services.AddSingleton<Geolocation>((s) => new Geolocation(s.GetRequiredService<IGeolocationService>()));

            services.AddSingleton<MainViewModel>();
            services.AddSingleton(s => new MainWindow() 
            {
                DataContext = s.GetRequiredService<MainViewModel>() 
            });
        })
        .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }
}
