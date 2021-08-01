using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace MinesweeperML.Cultures
{
    /// <summary>
    /// Wraps up XAML access to instance of WPFLocalize.Properties.Resources, list of
    /// available cultures, and method to change culture.
    /// </summary>
    // Source: https://www.codeproject.com/Articles/22967/WPF-Runtime-Localization
    public class CultureResources
    {
        // only fetch installed cultures once
        private static readonly bool HasFoundInstalledCultures = false;

        private static ObjectDataProvider resourceProvider;

        /// <summary>
        /// Gets the resource provider.
        /// </summary>
        /// <value>The resource provider.</value>
        public static ObjectDataProvider ResourceProvider
        {
            get
            {
                if (resourceProvider == null)
                {
                    resourceProvider = (ObjectDataProvider)App.Current.FindResource("Resources");
                }

                return resourceProvider;
            }
        }

        /// <summary>
        /// Gets list of available cultures, enumerated at startup.
        /// </summary>
        /// <value>The supported cultures.</value>
        public static List<CultureInfo> SupportedCultures { get; } = new List<CultureInfo>();

        static CultureResources()
        {
            if (!HasFoundInstalledCultures)
            {
                // Determine which cultures are available to this application
                Debug.WriteLine("Get Installed cultures:");
                foreach (string dir in Directory.GetDirectories(System.AppDomain.CurrentDomain.BaseDirectory))
                {
                    try
                    {
                        // see if this directory corresponds to a valid culture name
                        DirectoryInfo dirinfo = new DirectoryInfo(dir);
                        var culture = CultureInfo.GetCultureInfo(dirinfo.Name);

                        // determine if a resources dll exists in this directory that
                        // matches the executable name
                        if (dirinfo.GetFiles(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".resources.dll").Length > 0)
                        {
                            SupportedCultures.Add(culture);
                            Debug.WriteLine($"Found Culture: {culture.DisplayName} [{culture.Name}]");
                        }
                    }
                    catch (ArgumentException) // ignore exceptions generated for any unrelated directories in the bin folder
                    {
                    }
                }
                HasFoundInstalledCultures = true;
            }
        }

        /// <summary>
        /// Change the current culture used in the application. If the desired culture is
        /// available all localized elements are updated.
        /// </summary>
        /// <param name="culture">Culture to change to.</param>
        public static void SetCulture(CultureInfo culture)
        {
            // remain on the current culture if the desired culture cannot be found
            // - otherwise it would revert to the default resources set, which may or may
            // not be desired.
            if (SupportedCultures.Contains(culture))
            {
                Resources.Culture = culture;
                CultureInfo.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

                Properties.Settings.Default.UserCulture = culture;
                Properties.Settings.Default.Save();
                ResourceProvider.Refresh();
            }
            else
            {
                Debug.WriteLine($"Culture {culture} not available. Setting language to default.");
                if (!SupportedCultures.Contains(Properties.Settings.Default.DefaultCulture))
                {
                    SupportedCultures.Add(Properties.Settings.Default.DefaultCulture);
                }
                SetCulture(Properties.Settings.Default.DefaultCulture);
            }
        }

        /// <summary>
        /// The Resources ObjectDataProvider uses this method to get an instance of the
        /// WPFLocalize.Properties.Resources class.
        /// </summary>
        /// <returns>New resource.</returns>
        public Resources GetResourceInstance()
        {
            return new Resources();
        }
    }
}