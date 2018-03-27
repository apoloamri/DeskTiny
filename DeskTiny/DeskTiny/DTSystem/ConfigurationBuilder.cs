using DTCore.Tools.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DTCore.DTSystem
{
    public static class ConfigurationBuilder
    {
        public static IConfigurationRoot Configuration()
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("dtconfig.1.0.0.json", optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        public static class API
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("API");

            public static string[] AllowOrigins => ConfigurationSection.GetSection("AllowOrigins").Value.Split(',');
            public static int SessionTimeOut => Convert.ToInt32(ConfigurationSection.GetSection("SessionTimeOut").Value);
        }

        public static class Database
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Database");

            public static string ConnectionString
            {
                get
                {
                    string server = ConfigurationSection.GetSection("Server").Value;
                    string port = ConfigurationSection.GetSection("Port").Value;
                    string database = ConfigurationSection.GetSection("Database").Value;
                    string userId = ConfigurationSection.GetSection("UserId").Value;
                    string password = ConfigurationSection.GetSection("Password").Value;
                    string commandTimeout = ConfigurationSection.GetSection("CommandTimeout").Value;
                    string timeout = ConfigurationSection.GetSection("Timeout").Value;
                    string protocol = ConfigurationSection.GetSection("Protocol").Value;
                    string ssl = ConfigurationSection.GetSection("SSL").Value;
                    string sslMode = ConfigurationSection.GetSection("SslMode").Value;
                    string pooling = ConfigurationSection.GetSection("Pooling").Value;
                    string minPoolSize = ConfigurationSection.GetSection("MinPoolSize").Value;
                    string maxPoolSize = ConfigurationSection.GetSection("MaxPoolSize").Value;
                    string connectionLifeTime = ConfigurationSection.GetSection("ConnectionLifeTime").Value;

                    return
                        (!server.IsEmpty() ? $"Server={server};" : string.Empty) +
                        (!port.IsEmpty() ? $"Port={port};" : string.Empty) +
                        (!database.IsEmpty() ? $"Database={database};" : string.Empty) +
                        (!userId.IsEmpty() ? $"User Id={userId};" : string.Empty) +
                        (!password.IsEmpty() ? $"Password={password};" : string.Empty) +
                        (!commandTimeout.IsEmpty() ? $"CommandTimeout={commandTimeout};" : string.Empty) +
                        (!timeout.IsEmpty() ? $"Timeout={timeout};" : string.Empty) +
                        (!protocol.IsEmpty() ? $"Protocol={protocol};" : string.Empty) +
                        (!ssl.IsEmpty() ? $"SSL={ssl};" : string.Empty) +
                        (!sslMode.IsEmpty() ? $"SslMode={sslMode};" : string.Empty) +
                        (!pooling.IsEmpty() ? $"Pooling={pooling};" : string.Empty) +
                        (!minPoolSize.IsEmpty() ? $"MinPoolSize={minPoolSize};" : string.Empty) +
                        (!maxPoolSize.IsEmpty() ? $"MaxPoolSize={maxPoolSize};" : string.Empty) +
                        (!connectionLifeTime.IsEmpty() ? $"ConnectionLifeTime={connectionLifeTime};" : string.Empty);
                }
            }

            public static bool Migrate => Convert.ToBoolean(ConfigurationSection.GetSection("Migrate").Value);
        }
        
        public static class Logs
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("Logs");

            public static string Migration => ConfigurationSection.GetSection("Migration").Value;
            public static string System => ConfigurationSection.GetSection("System").Value;
        }
    }
}
