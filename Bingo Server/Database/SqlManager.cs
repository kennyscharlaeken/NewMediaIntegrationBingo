using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Database
{

    public class SqlManager
    {
        public ConnectionStringSettings ConnectionString { get; set; }

        private SqlTransaction _transaction = null;
        private SqlConnection _sqlcon=null;
        public DbConnection DbConnection { get{return _sqlcon;} }

        public SqlManager(string connectionstring)
        {
           ConnectionString = ConfigurationManager.ConnectionStrings[connectionstring];
        }

        public void connect()
        {
            close();
            _sqlcon = new SqlConnection(ConnectionString.ConnectionString);
            _sqlcon.Open();
        }
        public void close()
        {
            if (_sqlcon != null && _sqlcon.State == System.Data.ConnectionState.Open) _sqlcon.Close();
        }
        
        public DbDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            return ExecuteReader(sql,parameters.ToList());
        }
        public bool ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(sql, parameters.ToList());
        }
        public DbDataReader ExecuteReader(string sql, List<SqlParameter> parameters = null)
        {
            SqlDataReader rd = buildCommand(sql, parameters).ExecuteReader();
            return rd;
        }       
        public bool ExecuteNonQuery(string sql, List<SqlParameter> parameters)
        {
            int rows = buildCommand(sql, parameters).ExecuteNonQuery();
            if (rows > 0) return true; else return false;
        }
        public Object ExecuteReaderScalar(string sql, List<SqlParameter> parameters = null)
        {
            return buildCommand(sql, parameters).ExecuteScalar();
        }
        public Object ExecuteReaderScalar(string sql, params SqlParameter[] parameters)
        {
            return ExecuteReaderScalar(sql, parameters.ToList());
        }


        public SqlParameter makeParameter(string paramname,object value,SqlDbType datatype)
        {
            SqlParameter p = new SqlParameter() { ParameterName=paramname,Value=value,SqlDbType=datatype};
            return p;
        }
        public void useTranaction()
        {
            commitTransaction();
            _transaction = _sqlcon.BeginTransaction();
        }
        public void commitTransaction()
        {
            if (_transaction != null) _transaction.Commit();
            _transaction = null;
        }
        public void rollBackTransaction()
        {
            if (_transaction != null) _transaction.Rollback();
            _transaction = null;
        }
        


        #region BehindTheScene
        private SqlCommand buildCommand(string sql,List<SqlParameter> sqlparameters=null)
        {
            SqlCommand query = new SqlCommand(sql, _sqlcon);
            query.CommandType = System.Data.CommandType.Text;
            query.Transaction = _transaction;
            if (sqlparameters != null)
            {
                foreach (SqlParameter param in sqlparameters)
                {
                    query.Parameters.Add(param);
                }
            }
            return query;
        }
        #endregion


    }
}
