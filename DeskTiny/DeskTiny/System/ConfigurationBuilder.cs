using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DeskTiny.Tools
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
                        (!string.IsNullOrEmpty(server) ? $"Server={server};" : string.Empty) +
                        (!string.IsNullOrEmpty(port) ? $"Port={port};" : string.Empty) +
                        (!string.IsNullOrEmpty(database) ? $"Database={database};" : string.Empty) +
                        (!string.IsNullOrEmpty(userId) ? $"User Id={userId};" : string.Empty) +
                        (!string.IsNullOrEmpty(password) ? $"Password={password};" : string.Empty) +
                        (!string.IsNullOrEmpty(commandTimeout) ? $"CommandTimeout={commandTimeout};" : string.Empty) +
                        (!string.IsNullOrEmpty(timeout) ? $"Timeout={timeout};" : string.Empty) +
                        (!string.IsNullOrEmpty(protocol) ? $"Protocol={protocol};" : string.Empty) +
                        (!string.IsNullOrEmpty(ssl) ? $"SSL={ssl};" : string.Empty) +
                        (!string.IsNullOrEmpty(sslMode) ? $"SslMode={sslMode};" : string.Empty) +
                        (!string.IsNullOrEmpty(pooling) ? $"Pooling={pooling};" : string.Empty) +
                        (!string.IsNullOrEmpty(minPoolSize) ? $"MinPoolSize={minPoolSize};" : string.Empty) +
                        (!string.IsNullOrEmpty(maxPoolSize) ? $"MaxPoolSize={maxPoolSize};" : string.Empty) +
                        (!string.IsNullOrEmpty(connectionLifeTime) ? $"ConnectionLifeTime={connectionLifeTime};" : string.Empty);
                }
            }
        }

        public static class API
        {
            private static IConfigurationSection ConfigurationSection = Configuration().GetSection("API");

            public static int SessionTimeOut => Convert.ToInt32(ConfigurationSection.GetSection("SessionTimeOut").Value);
        }
    }
}
