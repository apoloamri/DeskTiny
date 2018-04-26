using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools.Extensions;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Tenderfoot.Database.System
{
    public class Connect
    {
        private string ConnectionString => TfSettings.Database.ConnectionString;
        private Provider Provider => TfSettings.Database.Provider;
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
                throw new TfException("SQL not provided.");
            }

            if (this.SqlCommand == null)
            {
                this.SqlCommand = this.SqlConnection.CreateCommand();
            }

            TfDebug.WriteLine("Executing query", sql);
            
            this.SqlCommand.CommandText = sql;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    this.SqlCommand.Parameters.Add(ConnectProvider.SqlParameter(parameter.Key, parameter.Value));
                }

                TfDebug.WriteLine("SQL Parameters", string.Join(Environment.NewLine, parameters));
            }
        }
    }

    public static class ConnectProvider
    {
        public static string Param()
        {
            switch (TfSettings.Database.Provider)
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
            string connection = TfSettings.Database.ConnectionString;

            TfDebug.WriteLine("Connecting database", connection);

            switch (TfSettings.Database.Provider)
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
            switch (TfSettings.Database.Provider)
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
