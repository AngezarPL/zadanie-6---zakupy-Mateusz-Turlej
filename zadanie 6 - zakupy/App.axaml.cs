using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using zadanie_6___zakupy.ViewModels;
using zadanie_6___zakupy.Views;
using zadanie6.Data;
using zadanie6.Models;

namespace zadanie_6___zakupy;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; private set; }
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddDbContext<zadanie6Context>(options => options.UseSqlite("Data Source=zadanie6.db"));
        services.AddTransient<MainViewModel>();
        return services.BuildServiceProvider();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Services = ConfigureServices();
        if (!Design.IsDesignMode)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<zadanie6Context>();
                context.Database.Migrate();
                if (!context.Purchases.Any())
                {
                    context.Add(new Purchase()
                    {
                        Title = "Kukurydza",
                        DateTime = DateTime.Now,
                        UnitPrice = 1m,
                        Amount = 3m
                    });
                    context.SaveChanges();
                }

            }
        }
        }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
