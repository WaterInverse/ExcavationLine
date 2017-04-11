using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Reflection;

namespace Common
{
    public class MySqlOperating
    {
        private string _errorString = "";
        private string _sqlString = "";
        private static string _connString = "DataSource=localhost;Port=3306;DataBase=jzmysql;uid=root;pwd=LGS199366;charset=utf8;";
        //private static string _connString = "DataSource=111.198.57.26;Port=7006;DataBase=cloud_aps;uid=xiecheng;pwd=xiecheng;charset=utf8;";

        private DbConnection _myConn;
        private DbCommand _myComm;
        private DbCommandBuilder _myCommBuild;
        private DbDataAdapter _myDataAda;



        public string ErrorString
        {
            get { return _errorString; }
        }
        public string SQLString
        {
            get
            {
                return _sqlString;
            }
            set
            {
                _sqlString = value;
            }
        }
        public static string ConnString
        {
            get
            {
                return _connString;
            }
        }
        public DbConnection myConn
        {
            get
            {
                return _myConn;
            }
            set
            {
                _myConn = value;
            }
        }
        public DbCommand myComm
        {
            get
            {
                return _myComm;
            }
            set
            {
                _myComm = value;
            }
        }
        public DbCommandBuilder myCommBuild
        {
            get
            {
                return _myCommBuild;
            }
            set
            {
                _myCommBuild = value;
            }
        }
        public DbDataAdapter myDataAda
        {
            get
            {
                return _myDataAda;
            }
            set
            {
                _myDataAda = value;
            }
        }

        /// <summary>
        /// 实例化数据库连接
        /// </summary>
        /// <param name="connStr">连接数据库字符串</param>
        public MySqlOperating()
        {
            try
            {
                myConn = new MySqlConnection(_connString);
                myConn.Open();
            }
            catch (Exception e)
            {
                _errorString = e.Message;
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            try
            {
                myConn = new MySqlConnection(_connString);
                myConn.Open();
            }
            catch (Exception e)
            {
                _errorString = e.Message;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (myConn != null)
            {
                myConn.Close();
                myConn.Dispose();
            }
            GC.Collect();
        }

        public DbDataReader GetData(string sqlString)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                MySqlDataReader reader = ((MySqlCommand)myComm).ExecuteReader();
                myTransaction.Commit();
                return reader;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return null;
            }
        }

        public DataSet GetData(string sqlString, string p_TableName)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                myDataAda = new MySqlDataAdapter((MySqlCommand)myComm);
                DataSet ds = new DataSet();
                myDataAda.Fill(ds, p_TableName);
                myTransaction.Commit();
                return ds;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行Sql语句，返回DataSet
        /// </summary>
        public DataSet GetDataSet(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                myDataAda = new MySqlDataAdapter((MySqlCommand)myComm);
                DataSet ds = new DataSet();
                myDataAda.Fill(ds);
                myTransaction.Commit();
                return ds;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行Sql语句，返回DataTable
        /// </summary>
        public DataTable GetDataTable(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                myDataAda = new MySqlDataAdapter((MySqlCommand)myComm);
                DataTable dt = new DataTable();
                myDataAda.Fill(dt);
                myTransaction.Commit();
                return dt;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return null;
            }
        }

        public Hashtable GetHashtable(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            DataTable dt = GetDataTable(sqlString, args);
            if (dt != null)
            {
                Hashtable ht = new Hashtable();
                foreach (DataRow row in dt.Rows)
                {
                    ht.Add(row[0], row[1]);
                }
                return ht;
            }
            else
            {
                return null;
            }
        }

        public DbDataReader GetDataReader(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                MySqlDataReader reader = ((MySqlCommand)myComm).ExecuteReader();
                myTransaction.Commit();
                return reader;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 由Sql语句返回DbCommand
        /// </summary>
        public DbCommand InitCommand(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            try
            {
                myComm = new MySqlCommand(sqlString);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                return myComm;
            }
            catch (Exception e)
            {
                _errorString = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 读取一个数据
        /// </summary>
        public object GetOneData(string sqlString)
        {
            SQLString = sqlString;
            MySqlDataReader reader = null;
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                reader = ((MySqlCommand)myComm).ExecuteReader();
                if (reader.Read())
                {
                    object obj = reader[0];
                    reader.Close();
                    return obj;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                reader.Close();
                _errorString = e.Message;
                return null;
            }
        }

        public object GetOneData(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            MySqlDataReader reader = null;
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                reader = ((MySqlCommand)myComm).ExecuteReader();
                if (reader.Read())
                {
                    object obj = reader[0];
                    reader.Close();
                    return obj;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                reader.Close();
                _errorString = e.Message;
                return null;
            }
        }

        /// <summary>
        /// 运行Sql语句，对数据库增、删、改
        /// </summary>
        public bool RunDataBase(string[] sqlString)
        {
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                foreach (string sql in sqlString)
                {
                    SQLString = sql;
                    myComm = new MySqlCommand(sql, (MySqlConnection)myConn);
                    myComm.ExecuteNonQuery();
                }
                myTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return false;
            }
        }

        public bool RunDataBase(string sqlString)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                myComm.ExecuteNonQuery();
                myTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return false;
            }
        }

        public bool RunDataBase(string[] sqlString, params object[] args)
        {
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                foreach (string sql in sqlString)
                {
                    SQLString = sql;
                    myComm = new MySqlCommand(sql, (MySqlConnection)myConn);
                    for (int i = 0; i < args.Length; i++)
                    {
                        MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                        myComm.Parameters.Add(p);
                        i++;
                    }
                    myComm.ExecuteNonQuery();
                }
                myTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                _errorString = e.Message;
                return false;
            }
        }

        public bool RunDataBase(string sqlString, params object[] args)
        {
            SQLString = sqlString;
            MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
            try
            {
                myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                for (int i = 0; i < args.Length; i++)
                {
                    MySqlParameter p = new MySqlParameter((string)args[i], args[i + 1]);
                    myComm.Parameters.Add(p);
                    i++;
                }
                myComm.ExecuteNonQuery();
                myTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                myTransaction.Rollback();
                myConn.Close();
                _errorString = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 手动封装的Mysql查询
        /// </summary>
        public static DataSet QryDataFromMysql(string tableName, string fieldList, string str_Where)
        {

            DataSet ds = new DataSet();
            DbConnection myConn = new MySqlConnection(_connString);
            string sqlString = "select " + fieldList + " FROM " + tableName + " " + str_Where;
            try
            {
                string[] fieldListArray = fieldList.Split(',');
                myConn.Open();
                MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
                DbCommand myComm = new MySqlCommand(sqlString, (MySqlConnection)myConn);
                DbDataAdapter myDataAda = new MySqlDataAdapter((MySqlCommand)myComm);
                myDataAda.Fill(ds);
                myTransaction.Commit();
            }
            catch (Exception e)
            {
                string _errorString = e.Message;
            }
            finally
            {
                if (myConn != null)
                {
                    myConn.Close();
                    myConn.Dispose();

                }
            }
            return ds;
        }

        /// <summary>
        /// 手动封装的Mysql查询
        /// </summary>
        public static DataSet QryDataFromMysql(string sql)
        {

            DataSet ds = new DataSet();
            DbConnection myConn = new MySqlConnection(_connString);
            try
            {
                myConn.Open();
                MySqlTransaction myTransaction = ((MySqlConnection)myConn).BeginTransaction();
                DbCommand myComm = new MySqlCommand(sql, (MySqlConnection)myConn);
                DbDataAdapter myDataAda = new MySqlDataAdapter((MySqlCommand)myComm);
                myDataAda.Fill(ds);
                myTransaction.Commit();
            }
            catch (Exception e)
            {
                string _errorString = e.Message;
            }
            finally
            {
                if (myConn != null)
                {
                    myConn.Close();
                    myConn.Dispose();

                }
            }
            return ds;
        }

        /// <summary>
        /// 将dataTable转化为具有相同属性的类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TableToEntity<T>(string sql) where T : class,new()
        {
            DataSet ds = QryDataFromMysql(sql);
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (!(row[p.Name] is DBNull))
                    {
                        if (p.PropertyType == typeof(int))
                        {
                            p.SetValue(entity, Convert.ToInt32(row[p.Name]), null);
                        }
                        else
                        {
                            p.SetValue(entity, Convert.ToString(row[p.Name]), null);
                        }

                    }
                }
                list.Add(entity);
            }
            return list;
        }

    }
}
