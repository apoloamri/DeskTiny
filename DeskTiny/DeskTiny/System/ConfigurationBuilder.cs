using Microsoft.Extensions.Configuration;
using System.IO;

namespace DeskTiny.Tools
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
                string server = Configuration().GetSection("Database").GetSection("Server").Value;
                string port = Configuration().GetSection("Database").GetSection("Port").Value;
                string database = Configuration().GetSection("Database").GetSection("Database").Value;
                string userId = Configuration().GetSection("Database").GetSection("UserId").Value;
                string password = Configuration().GetSection("Database").GetSection("Password").Value;
                string commandTimeout = Configuration().GetSection("Database").GetSection("CommandTimeout").Value;
                string timeout = Configuration().GetSection("Database").GetSection("Timeout").Value;
                string protocol = Configuration().GetSection("Database").GetSection("Protocol").Value;
                string ssl = Configuration().GetSection("Database").GetSection("SSL").Value;
                string sslMode = Configuration().GetSection("Database").GetSection("SslMode").Value;
                string pooling = Configuration().GetSection("Database").GetSection("Pooling").Value;
                string minPoolSize = Configuration().GetSection("Database").GetSection("MinPoolSize").Value;
                string maxPoolSize = Configuration().GetSection("Database").GetSection("MaxPoolSize").Value;
                string connectionLifeTime = Configuration().GetSection("Database").GetSection("ConnectionLifeTime").Value;

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
}
