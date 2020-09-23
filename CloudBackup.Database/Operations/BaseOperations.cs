using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CloudBackup.Database.Operations
{
    public class BaseOperations
    {
        public IDbConnection _connection;
        public BaseOperations(string connectionString)
        {
           _connection = new NpgsqlConnection(connectionString);
        }

    }
}
