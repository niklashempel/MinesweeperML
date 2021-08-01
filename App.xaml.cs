using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinesweeperML.Business.AutoMapper;
using MinesweeperML.Business.Constants;
using MinesweeperML.Business.Database;
using MinesweeperML.Business.Database.DbContexts;
using MinesweeperML.Business.Interfaces;
using MinesweeperML.Views;
using MinesweeperML.ViewsModel;
using Unity;
using Unity.Injection;
using Unity.RegistrationByConvention;

namespace MinesweeperML
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;
        private IErrorHandler errorHandler;

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public IMapper Mapper { get; private set; }

        /// <inheritdoc />
        protected override void OnExit(ExitEventArgs e)
        {
            container.Dispose();
            base.OnExit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event
        /// data.
        /// </param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Check if database exists
            await CheckDirectoriesAsync();

            // Configure dependency injection container.
            container = new UnityContainer();
            ConfigureContainer(container);

            errorHandler = container.Resolve<IErrorHandler>();

            var viewModel = container.Resolve<StartWindowViewModel>();
            var view = container.Resolve<StartWindow>();
            view.DataContext = viewModel;
            view.Show();
        }

        private static async Task CheckDirectoriesAsync()
        {
            if (!File.Exists(StringConstants.MinesweeperDbPath))
            {
                await DbCreator.CreateAsync();
            }
        }

        private static void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),
                (c) => WithMappings.FromMatchingInterface(c));

            // AutoMapper
            var mapper = AutoMapperConfiguration.Configure();
            container.RegisterInstance(mapper);

            // Database
            container.RegisterType<MinesweeperDbContext>(
                new InjectionConstructor(new DbContextOptionsBuilder<MinesweeperDbContext>()
                .UseSqlite(StringConstants.MinesweeperDbConnectionString).Options));
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            errorHandler.HandleError(e.Exception);
            e.Handled = true;
        }
    }
}