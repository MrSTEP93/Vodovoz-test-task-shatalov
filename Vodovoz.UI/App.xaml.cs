using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using Vodovoz.Data.Config;
using Vodovoz.Data.Infrastructure;
using Vodovoz.Data.Repositories;
using Vodovoz.Data.Services;
using Vodovoz.Domain.Interfaces;
using Vodovoz.Services;
using Vodovoz.UI.ViewModels;

namespace Vodovoz.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена в конфигурации.");

                    var dbConfig = new DatabaseConfig
                    {
                        ConnectionString = connectionString
                    };
                    services.AddSingleton(dbConfig);
                    services.AddSingleton<ISessionFactoryProvider, SessionFactoryProvider>();

                    services.AddTransient<IClientRepository, ClientRepository>();
                    services.AddTransient<IEmployeeRepository, EmployeeRepository>();
                    services.AddTransient<IOrderRepository, OrderRepository>();

                    services.AddTransient<IClientService, ClientService>();
                    services.AddTransient<IEmployeeService, EmployeeService>();
                    services.AddTransient<IOrderService, OrderService>();

                    services.AddTransient<MainViewModel>();
                    services.AddTransient<EmployeesViewModel>();
                    services.AddTransient<ClientsViewModel>();
                    services.AddTransient<OrdersViewModel>();
                    services.AddTransient<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            _host.Services.GetRequiredService<ISessionFactoryProvider>();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }

}
