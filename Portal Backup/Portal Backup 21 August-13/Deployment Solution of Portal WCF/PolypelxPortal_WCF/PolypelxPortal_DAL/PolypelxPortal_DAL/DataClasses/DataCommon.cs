using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PolypelxPortal_BAL.BasicClasses;

namespace PolypelxPortal_DAL.DataClasses
{
    public class DataCommon
    {
        #region *****************************Variables**************************************
        DataTable dt = new DataTable();
        public SqlDataAdapter objSqlDa;
        public DataSet objDs;
        public SqlDataReader objSqlDr;
        ConnectionClass.Connection objConnectionClass = new ConnectionClass.Connection();

        #endregion

        #region *****************************Functions**************************************

        public DataTable GetDataTableWithProc(SqlCommand command)
        {
            try
            {
                objConnectionClass.OpenConnection();
                command.Connection = objConnectionClass.PolypexSqlConnection;
                command.CommandTimeout = 60;
                command.CommandType = CommandType.StoredProcedure;
                objSqlDa = new SqlDataAdapter();
                dt = new DataTable();
                objSqlDa.SelectCommand = command;
                objSqlDa.Fill(dt);
                objConnectionClass.CloseConnection();
                objConnectionClass.DisposeConnection();
            }
            catch(Exception ex)
            {
               
            }
            return dt;
        }

        public DataTable GetDataTableWithQuery(SqlCommand command)
        {
            try
            {
                objConnectionClass.OpenConnection();
                command.Connection = objConnectionClass.PolypexSqlConnection; ;
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                objSqlDa = new SqlDataAdapter();
                dt = new DataTable();
                objSqlDa.SelectCommand = command;
                objSqlDa.Fill(dt);
                objConnectionClass.CloseConnection();
                objConnectionClass.DisposeConnection();
            }
            catch { }
            return dt;
        }

        public DataSet GetDataSetWithQuery(SqlCommand command)
        {
            try
            {
                objConnectionClass.OpenConnection();
                command.Connection = objConnectionClass.PolypexSqlConnection;
                command.CommandTimeout = 60;
                command.CommandType = CommandType.Text;
                objSqlDa = new SqlDataAdapter();
                objDs = new DataSet();
                objSqlDa.SelectCommand = command;
                objSqlDa.Fill(objDs);
                objConnectionClass.CloseConnection();
                objConnectionClass.DisposeConnection();
            }
            catch { }
            return objDs;
        }

        public SqlCommand ExecuteSqlProcedure(SqlCommand command)
        {
            try
            {                
                objConnectionClass.OpenConnection();
                command.Connection = objConnectionClass.PolypexSqlConnection;
                command.CommandTimeout = 60;
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                objConnectionClass.CloseConnection();
                objConnectionClass.DisposeConnection();
            }
            catch (Exception ex) { }
            return command;
        }

        public DataTable ExecuteSqlQry(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, objConnectionClass.PolypexSqlConnection);
                objConnectionClass.CloseConnection();
                objConnectionClass.DisposeConnection();
                da.Fill(dt);
                da.Dispose();
            }
            catch { }
            return dt;
        }

        #endregion
    }
}
