using DTCore.DTSystem;
using DTCore.DTSystem.Diagnostics;
using DTCore.Tools.Extensions;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DTCore.Database.System
{
    public class Connect
    {
        private string ConnectionString => Settings.Database.ConnectionString;
        private Provider Provider => Settings.Database.Provider;
        public dynamic SqlConnection { get; set; } = null;
        public dynamic SqlCommand { get; set; } = null;
        public dynamic SqlDataReader { get; set; } = null;
        public dynamic SqlTransaction { get; set; } = null;
        
        public Connect(string sql, Dictionary<string, object> parameters)
        {
            this.SqlConnection = ConnectProvider.SqlConnection();
            this.WriteSql(sql, parameters);
        }

        public Connect()
        {
            this.SqlConnection = ConnectProvider.SqlConnection();
        }

        public void WriteSql(string sql, Dictionary<string, object> parameters)
        {
            if (sql.IsEmpty())
            {
                throw new DTException("SQL not provided.");
            }

            if (this.SqlCommand == null)
            {
                this.SqlCommand = this.SqlConnection.CreateCommand();
            }

            DTDebug.WriteLine("Executing query", sql);
            
            this.SqlCommand.CommandText = sql;

            foreach (var parameter in parameters)
            {
                this.SqlCommand.Parameters.Add(ConnectProvider.SqlParameter(parameter.Key, parameter.Value));
            }

            DTDebug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
        }
    }

    public static class ConnectProvider
    {
        public static string Param()
        {
            switch (Settings.Database.Provider)
            {
                case Provider.MySql:
                case Provider.SqlServer:
                    return "@";

                case Provider.Postgres:
                default:
                    return ":";
            }
        }

        public static dynamic SqlConnection()
        {
            string connection = Settings.Database.ConnectionString;

            DTDebug.WriteLine("Connecting database", connection);

            switch (Settings.Database.Provider)
            {
                case Provider.MySql:
                    return new MySqlConnection(connection);
                case Provider.Postgres:
                    return new NpgsqlConnection(connection);
                case Provider.SqlServer:
                    return new SqlConnection(connection);
                default:
                    return new NpgsqlConnection(connection);
            }
        }

        public static dynamic SqlParameter(string key, object value)
        {
            switch (Settings.Database.Provider)
            {
                case Provider.MySql:
                    return new MySqlParameter(key, value);
                case Provider.Postgres:
                    return new NpgsqlParameter(key, value);
                case Provider.SqlServer:
                    return new SqlParameter(key, value);
                default:
                    return new NpgsqlParameter(key, value);
            }
        }
    }
}
