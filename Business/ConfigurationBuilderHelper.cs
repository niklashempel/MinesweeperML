using System.IO;
using Microsoft.Extensions.Configuration;

namespace MinesweeperML.Business
{
    /// <summary>
    /// The configuration builder helper class.
    /// </summary>
    public class ConfigurationBuilderHelper
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>The configuration <see cref="IConfigurationRoot" />.</returns>
        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}