using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;

namespace MC
{
    class resultsDatabase
    {
        protected OdbcConnection connection;

        protected resultsDatabase()
        {
            connection = new OdbcConnection();
        }

        protected void connect()
        {
            string localConnectionString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=" + MCParameters.databaseFileName + ";Uid=Admin;Pwd=;";
            connection.ConnectionString = localConnectionString;
            connection.Open();
        }
    }
}
