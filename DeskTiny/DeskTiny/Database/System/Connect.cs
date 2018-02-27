using DeskTiny.System;
using DeskTiny.System.Diagnostics;
using DeskTiny.Tools;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DeskTiny.Database.System
{
    public class Connect
    {
        public NpgsqlConnection NpgsqlConnection { get; set; } = null;
        public NpgsqlCommand NpgsqlCommand { get; set; } = null;
        public NpgsqlDataReader NpgsqlDataReader { get; set; } = null;

        public Connect(string sql, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new CustomException("SQL not provided.");
            }

            string connectionString = ConfigurationBuilder.ConnectionString;
            
            Debug.WriteLine("Connecting database", connectionString);
            
            this.NpgsqlConnection = new NpgsqlConnection(connectionString);

            Debug.WriteLine("Executing query", sql);

            this.NpgsqlCommand = new NpgsqlCommand(sql, NpgsqlConnection);

            foreach (var parameter in parameters)
            {
                this.NpgsqlCommand.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }

            Debug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
        }
    }
}
