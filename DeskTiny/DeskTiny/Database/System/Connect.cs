using DTCore.DTSystem;
using DTCore.DTSystem.Diagnostics;
using DTCore.Tools.Extensions;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DTCore.Database.System
{
    public class Connect
    {
        private string ConnectionString => ConfigurationBuilder.Database.ConnectionString;
        public NpgsqlConnection NpgsqlConnection { get; set; } = null;
        public NpgsqlCommand NpgsqlCommand { get; set; } = null;
        public NpgsqlDataReader NpgsqlDataReader { get; set; } = null;
        public NpgsqlTransaction NpgsqlTransaction { get; set; } = null;

        public Connect(string sql, Dictionary<string, object> parameters)
        {
            DTDebug.WriteLine("Connecting database", ConnectionString);
            this.NpgsqlConnection = new NpgsqlConnection(ConnectionString);
            this.WriteSql(sql, parameters);
        }

        public Connect()
        {
            DTDebug.WriteLine("Connecting database", ConnectionString);
            this.NpgsqlConnection = new NpgsqlConnection(ConnectionString);
        }

        public void WriteSql(string sql, Dictionary<string, object> parameters)
        {
            if (sql.IsEmpty())
            {
                throw new DTException("SQL not provided.");
            }

            if (this.NpgsqlCommand == null)
            {
                this.NpgsqlCommand = this.NpgsqlConnection.CreateCommand(); //new NpgsqlCommand(sql, this.NpgsqlConnection);
            }

            DTDebug.WriteLine("Executing query", sql);
            
            this.NpgsqlCommand.CommandText = sql;

            foreach (var parameter in parameters)
            {
                this.NpgsqlCommand.Parameters.Add(new NpgsqlParameter(parameter.Key, parameter.Value));
            }

            DTDebug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
        }
    }
}
