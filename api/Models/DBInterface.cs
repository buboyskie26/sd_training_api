using api.App_Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class DBInterface
    {
        #region "variables"
        private string sErrMessage = string.Empty;
        private ErrorLogs _logger = new ErrorLogs();
        #endregion

        #region "properties"
        public string ErrorMessage
        {
            get { return sErrMessage; }
        }
        #endregion


        #region "public methods"
        public string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        }

        protected int ExecuteCUD(string sProc, SqlParameterCollection oArrParam)
        {
            int iRecordsAffected = 0;
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter oParam in oArrParam)
                    {
                        cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                    }
                    iRecordsAffected = cmd.ExecuteNonQuery();
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteCUD | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");

                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteCUD | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
            }

            return iRecordsAffected;
        }

        protected int ExecuteCUD(string sProc)
        {
            int iRecordsAffected = 0;
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    iRecordsAffected = cmd.ExecuteNonQuery();
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteCUD | SP: {sProc} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteCUD | SP: {sProc} | {sErrMessage}");
                }
            }

            return iRecordsAffected;
        }

        protected int ExecuteInsertWithIdentity(string sProc, SqlParameterCollection oArrParam)
        {
            int iReturnIdentity = 0;
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter oParam in oArrParam)
                    {
                        cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                    }

                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    iReturnIdentity = Convert.ToInt32(cmd.Parameters["@id"].Value);
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteInsertWithIdentity | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteInsertWithIdentity | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
            }

            return iReturnIdentity;
        }

        public DataTable ExecuteRead(string sProc, SqlParameterCollection oArrParam)
        {
            DataTable dt = new DataTable();
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter oParam in oArrParam)
                    {
                        cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteRead | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteRead | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
            }

            return dt;
        }

        public DataTable ExecuteRead(string sProc)
        {
            DataTable dt = new DataTable();
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteRead | SP: {sProc} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteRead | SP: {sProc} | {sErrMessage}");
                }
            }

            return dt;
        }

        protected int ExecuteScalar(string sProc, SqlParameterCollection oArrParam)
        {
            int _return = 0;
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter oParam in oArrParam)
                    {
                        cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                    }

                    _return = Convert.ToInt32(cmd.ExecuteScalar());
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteScalar | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteScalar | SP: {sProc}, {oArrParam.ToString()} | {sErrMessage}");
                }
            }

            return _return;
        }
        protected int ExecuteScalar(string sProc)
        {
            int _return = 0;
            sErrMessage = String.Empty;
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    _return = Convert.ToInt32(cmd.ExecuteScalar());
                    cnn.Close();
                }
                catch (SqlException sqlerr)
                {
                    sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
                    _logger.createLogs($"SQL Exception in ExecuteScalar | SP: {sProc} | {sErrMessage}");
                }
                catch (Exception err)
                {
                    sErrMessage = " Runtime Error: " + err.Message;
                    _logger.createLogs($"Exception in ExecuteScalar | SP: {sProc} | {sErrMessage}");
                }
            }

            return _return;
        }
        #endregion


        #region "private methods"
        #endregion
    }
}