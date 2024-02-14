using api.Models;
using api.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace api.App_Utility.Data
{
    public class UsersUtils : DBInterface
    {
        private ErrorLogs _logger = new ErrorLogs();
        public UsersReturnDTO signin(UsersSigninDTO dto)
        {
            var result = new UsersReturnDTO();


            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@username", dto.username);
            _params.AddWithValue("@password", Crypto.Encryption(dto.password));

            DataTable dt = ExecuteRead("dbo.sp_users_sign_in", _params);

            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow row in dt.Rows)
            {

                result.Id = Convert.ToInt32(row["id"].ToString());
                result.Username = row["username"].ToString();
                result.Name = row["name"].ToString();
                result.IsActive = Convert.ToBoolean(row["isActive"].ToString());

                if (row["createdDate"].ToString() != "")
                    result.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());

                if (row["modifiedDate"].ToString() != "")
                    result.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());
            }

            return result;
        }

        public List<UsersReturnDTO> getAll()
        {
            List<UsersReturnDTO> result = new List<UsersReturnDTO>();
            DataTable dt = getByIdentifier("all", "");

            foreach (DataRow row in dt.Rows)
            {
                UsersReturnDTO dto = new UsersReturnDTO();

                dto.Id = Convert.ToInt32(row["id"].ToString());
                dto.Username = row["username"].ToString();
                dto.Name = row["name"].ToString();
                dto.Email = row["email"].ToString();

                dto.IsActive = Convert.ToBoolean(row["isActive"].ToString());

                if (row["createdDate"].ToString() != "")
                    dto.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());

                if (row["modifiedDate"].ToString() != "")
                    dto.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());

                result.Add(dto);
            }

            return result;
        }

        public UsersReturnDTO getById(int? id)
        {
            UsersReturnDTO result = new UsersReturnDTO();
            DataTable dt = getByIdentifier("id", id.ToString());
            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow row in dt.Rows)
            {

                result.Id = Convert.ToInt32(row["id"].ToString());
                result.Username = row["username"].ToString();
                result.Name = row["name"].ToString();
                result.Email = row["email"].ToString();
                result.IsActive = Convert.ToBoolean(row["isActive"].ToString());
                if (row["createdDate"].ToString() != "")
                    result.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                if (row["modifiedDate"].ToString() != "")
                    result.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());
            }

            return result;
        }

        public UsersReturnDTO getByEmail(string email)
        {
            UsersReturnDTO result = new UsersReturnDTO();
            DataTable dt = getByIdentifier("email", email);

            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow row in dt.Rows)
            {

                result.Id = Convert.ToInt32(row["id"].ToString());
                result.Username = row["username"].ToString();
                result.Name = row["name"].ToString();
                result.Email = row["email"].ToString();
                result.IsActive = Convert.ToBoolean(row["isActive"].ToString());
                if (row["createdDate"].ToString() != "")
                    result.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                if (row["modifiedDate"].ToString() != "")
                    result.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());
            }

            return result;
        }

        public void create(UsersCreateDTO dto, int user_id)
        {

            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@username", dto.username);
            _params.AddWithValue("@password", Crypto.Encryption(dto.password));
            _params.AddWithValue("@name", dto.name);
            _params.AddWithValue("@email", dto.email);
            _params.AddWithValue("@user_id", user_id);

            ExecuteRead("dbo.sp_users_create", _params);


            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"UesrsUtils | update | {JsonConvert.SerializeObject(dto) + " " + ErrorMessage}");
            }
        }

        public void update(UsersUpdateDTO dto, int id, int user_id)
        {

            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@id", id);
            _params.AddWithValue("@name", dto.name);
            _params.AddWithValue("@is_active", dto.is_active);
            _params.AddWithValue("@email", dto.email);
            _params.AddWithValue("@user_id", user_id);

            ExecuteRead("dbo.sp_users_update", _params);

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"UsersUtils | update | {JsonConvert.SerializeObject(dto) + " " + ErrorMessage}");
            }

        }

        public void changeStatus(int id, int user_id, bool isActive)
        {

            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@id", id);
            _params.AddWithValue("@is_active", isActive);
            _params.AddWithValue("@user_id", user_id);
            this.ExecuteRead("dbo.sp_users_change_status", _params);
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"UsersUtils | change Status | {"id: " + id.ToString() + " " + ErrorMessage}");
            }
        }

        public void forgotPassword(int id, string password)
        {

            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@id", id);
            _params.AddWithValue("@password", Crypto.Encryption(password));

            this.ExecuteRead("dbo.sp_users_forgot", _params);
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"UsersUtils | change Status | {"id: " + id.ToString() + " password: " + password + " " + ErrorMessage}");
            }

        }

        public string validateUsername(string value)
        {

            DataTable dt = getByIdentifier("username", value);
            if (dt.Rows.Count == 0)
                return null;

            return "Username already exists. ";
        }

        public string validateEmail(string value)
        {

            DataTable dt = getByIdentifier("email", value);
            if (dt.Rows.Count == 0)
                return null;

            return "Email already exists. ";
        }

        public DataTable getByIdentifier(string identifier, string value)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@identifier", identifier);
            _params.AddWithValue("@value", value);

            DataTable dt = ExecuteRead(@"dbo.sp_users_get", _params);

            return dt;
        }

        public string getToken(int userid)
        {
            // Create payload
            var _payloads = new Dictionary<string, string>();
            _payloads.Add("api_key", "sds");
            _payloads.Add("api_secret", "sdssecret");
            _payloads.Add("userid", userid.ToString());

            string token = JwtManager.GenerateToken(_payloads, 300);

            return token;
        }
    }
}