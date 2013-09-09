using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PolypelxPortal_DAL.ConnectionClass
{
    public class Connection
    {
        #region *****************************Variables**************************************

        public SqlConnection PolypexSqlConnection;
        string _ConnectionString;

        #endregion

        #region *****************************Functions**************************************

        public Connection()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["Polyplex_DB"].ToString();
        }

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }

        public string _PolypexConnectionString
        {
            get
            {
                return ConnectionString;
            }
        }

        public void OpenConnection()
        {

            if (PolypexSqlConnection == null)
            {
                PolypexSqlConnection = new SqlConnection(_ConnectionString);
            }
            if (PolypexSqlConnection.State == System.Data.ConnectionState.Closed)
            {
                PolypexSqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (PolypexSqlConnection == null)
            {
                PolypexSqlConnection = new SqlConnection(_ConnectionString);
            }
            if (PolypexSqlConnection.State == System.Data.ConnectionState.Open)
            {
                PolypexSqlConnection.Close();
            }
        }

        public void DisposeConnection()
        {
            if (PolypexSqlConnection == null)
            {
                PolypexSqlConnection = new SqlConnection(_ConnectionString);
            }
            if (PolypexSqlConnection == null)
            {
                PolypexSqlConnection.Dispose();
            }
        }

        #endregion
    }
}
