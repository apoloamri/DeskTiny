using Microsoft.Extensions.Configuration;
using System.IO;

namespace DeskTiny
{
    public static class ConfigurationBuilder
    {
        public static IConfigurationRoot Configuration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("desktiny_v1.0.0.json", optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        public static string ConnectionString
        {
            get
            {
                return Configuration().GetSection("Database").GetSection("ConnectionString").Value;
            }
        }
    }
}
