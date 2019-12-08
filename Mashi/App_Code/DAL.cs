using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mashi.App_Code
{
    public class DAL
    {
        public string errortext = "";
        SqlConnection con;
        SqlDataAdapter adp;
        SqlCommand cmd;
        string constr, Query;
        long r;

        public long lastid = 0;
        public DAL()
        {
            con = new SqlConnection();
            cmd = new SqlCommand();
            adp = new SqlDataAdapter();
            lastid = 0;
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
        }
        public void connect()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
                con.Open();
            }

        }
        public void disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
                SqlConnection.ClearPool(con);
            }
        }
        public bool checkconnection()
        {
            try
            {
                con.Open();
                con.Close();
            }
            catch (Exception ex)
            {
                errortext = ex.Message + con.ConnectionString;
                return (false);
            }
            return (true);
        }
        public long UpdateTable(string tblname, string[,] data, string[,] @where)
        {

            string sql = null;
            this.connect();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;


            int j = 0;
            string fldnames = "";
            string fldvalues = "";
            string wfldnames = "";

            //read fields
            for (j = 0; j < data.GetLength(0); j++)
            {
                if (j == 0)
                {
                    fldnames = data[0, j] + "=@" + data[0, j];
                }
                else
                {
                    fldnames = fldnames + "," + data[0, j] + "=@" + data[0, j];
                }
            }

            //read where fields
            wfldnames = @where[0, 0] + "=@" + @where[0, 0];

            sql = "update " + tblname + " set " + fldnames + " where " + wfldnames;
            cmd.CommandText = sql;

            //read fields values
            for (j = 0; j < data.GetLength(0); j++)
            {
                cmd.Parameters.AddWithValue("@" + data[0, j], data[1, j]);
            }

            cmd.Parameters.AddWithValue("@" + @where[0, 0], @where[1, 0]);


            try
            {
                r = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
                r = 0;
            }
            finally
            {
                this.disconnect();
            }
            return (r);
        }

        #region SQL Query
        public long SQLExecuteNonQuery(string sql)
        {
            try
            {
                this.connect();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                r = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errortext = ex.Message;
                r = -1;
            }
            finally
            {
                this.disconnect();
            }

            return (r);
        }
        public long SQLExecuteScalar(string sql)
        {
            try
            {
                this.connect();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                r = (long)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
                r = 0;
            }
            finally
            {
                this.disconnect();
            }
            return (r);
        }
        public DataTable SQLGetDataTablw(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                this.connect();
                cmd.CommandTimeout = 99999;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                adp.SelectCommand = cmd;
                adp.Fill(ds);
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
            }
            finally
            {
                this.disconnect();
            }
            return (ds.Tables[0]);
        }
        public DataSet SQLGetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                this.connect();
                cmd.CommandTimeout = 99999;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                adp.SelectCommand = cmd;
                adp.Fill(ds);
            }
            catch (Exception ex)
            {
                this.errortext = ex.Message;
            }
            finally
            {
                this.disconnect();
            }
            return ds;
        }
        #endregion

        #region StoredProcedures
        public int SPExecuteNonQuery(string proc, SqlParameter[] parameters)
        {
            int IsSuccess = 0;
            try
            {

                if (proc != null)
                {

                    SqlCommand mycmd = new SqlCommand(proc, con);
                    mycmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {

                            mycmd.Parameters.Add(param);
                        }
                    }
                    this.connect();
                    IsSuccess = mycmd.ExecuteNonQuery();
                    this.disconnect();
                    return IsSuccess;

                }
                else
                {
                    IsSuccess = 0;
                    return IsSuccess;
                }
            }
            catch (SqlException ex)
            {
                IsSuccess = 0;
                return IsSuccess;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
            finally
            {
                this.disconnect();
            }
        }
        public DataSet SPExecuteDataset(string proc, SqlParameter[] parameters, string virtualtable)
        {
            try
            {
                if (proc != null)
                {
                    this.connect();
                    SqlCommand mycmd = new SqlCommand(proc, con);
                    mycmd.CommandType = CommandType.StoredProcedure;
                    mycmd.CommandTimeout = 2400;
                    DataSet myds = new DataSet();
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            mycmd.Parameters.Add(param);
                        }
                    }
                    this.connect();
                    SqlDataAdapter mysda = new SqlDataAdapter(mycmd);
                    mysda.Fill(myds, virtualtable);
                    return myds;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.disconnect();
            }

        }
        public DataTable SPExecuteDataTable(string proc, SqlParameter[] parameters, string virtualtable)
        {
            try
            {
                if (proc != null)
                {
                    DataSet myds = new DataSet();
                    try
                    {
                        this.connect();
                        SqlCommand mycmd = new SqlCommand(proc, con);
                        mycmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            foreach (SqlParameter param in parameters)
                            {
                                mycmd.Parameters.Add(param);
                            }
                        }

                        SqlDataAdapter mysda = new SqlDataAdapter(mycmd);
                        mysda.SelectCommand.CommandTimeout = 240;
                        mysda.Fill(myds, virtualtable);
                    }
                    catch (Exception ex)
                    {
                        errortext = ex.Message;
                    }
                    finally
                    {
                        this.disconnect();
                    }

                    return (myds.Tables[virtualtable]);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.disconnect();
            }

        }
        public long SPExecuteScalar(string proc, SqlParameter[] parameters)
        {
            long r = 0;
            try
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

                if (proc != null)
                {

                    SqlCommand mycmd = new SqlCommand(proc, con);
                    mycmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {

                            mycmd.Parameters.Add(param);
                        }
                    }

                    con.Open();
                    mycmd.CommandTimeout = 99999;
                    r = (int)mycmd.ExecuteScalar();
                    con.Close();
                    return r;

                }
                else
                {
                    r = 0;
                    return r;
                }
            }
            catch (SqlException ex)
            {
                r = 0;
                return r;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return 0;
            }
            finally
            {
                this.disconnect();
            }
        }
        #endregion

    }
}