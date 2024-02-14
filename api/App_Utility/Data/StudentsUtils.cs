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
    public class StudentsUtils : DBInterface
    {
        private ErrorLogs _logger = new ErrorLogs();

        public List<StudentsReturnDTO> getAll()
        {
            var result = new List<StudentsReturnDTO>();
            DataTable dt = getByIdentifier("all", "");

            foreach (DataRow row in dt.Rows)
            {
                var dto = new StudentsReturnDTO();

                dto.Id = Convert.ToInt32(row["id"].ToString());
                dto.FirstName = row["firstName"].ToString();
                dto.LastName = row["lastName"].ToString();
                dto.MiddleName = row["middleName"].ToString();
                dto.Address = row["address"].ToString();
                dto.Age = Convert.ToInt32(row["age"].ToString());

                dto.IsActive = Convert.ToBoolean(row["isActive"].ToString());

                if (row["createdDate"].ToString() != "")
                    dto.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());

                if (row["modifiedDate"].ToString() != "")
                    dto.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());

                result.Add(dto);
            }

            return result;
        }
        public StudentsReturnDTO getById(int? id)
        {
            var dto = new StudentsReturnDTO();

            DataTable dt = getByIdentifier("id", id.ToString());

            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow row in dt.Rows)
            {

                dto.Id = Convert.ToInt32(row["id"].ToString());
                dto.FirstName = row["firstName"].ToString();
                dto.LastName = row["lastName"].ToString();
                dto.MiddleName = row["middleName"].ToString();
                dto.Gender = row["gender"].ToString();
                dto.Address = row["address"].ToString();
                dto.Course = row["course"].ToString();
                dto.Age = Convert.ToInt32(row["age"].ToString());
                
                dto.IsActive = Convert.ToBoolean(row["isActive"].ToString());

                if (row["createdDate"].ToString() != "")
                    dto.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());

                if (row["modifiedDate"].ToString() != "")
                    dto.ModifiedDate = Convert.ToDateTime(row["modifiedDate"].ToString());
            }

            return dto;
        }


        public void createStudent(StudentCreateDTO dto)
        {

            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@first_name", dto.FirstName);
            _params.AddWithValue("@last_name", dto.LastName);
            _params.AddWithValue("@middle_name", dto.MiddleName);
            _params.AddWithValue("@gender", dto.Gender);
            _params.AddWithValue("@age", dto.Age);
            _params.AddWithValue("@address", dto.Address);
            _params.AddWithValue("@course", dto.Course);
            _params.AddWithValue("@user_id", 1);

            this.ExecuteRead("dbo.sp_students_create", _params);


            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"StudentsUtils | create | {JsonConvert.SerializeObject(dto) + " " + ErrorMessage}");
            }
        }

        public void studentUpdate(StudentCreateDTO dto, int id)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@id", id);
            _params.AddWithValue("@first_name", dto.FirstName);
            _params.AddWithValue("@last_name", dto.LastName);
            _params.AddWithValue("@middle_name", dto.MiddleName);
            _params.AddWithValue("@gender", dto.Gender);
            _params.AddWithValue("@age", dto.Age);
            _params.AddWithValue("@address", dto.Address);
            _params.AddWithValue("@course", dto.Course);
            _params.AddWithValue("@user_id", 1);
            _params.AddWithValue("@is_active", dto.IsActive);

            ExecuteRead("dbo.sp_students_update", _params);

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                _logger.createLogs($"StudentsUtils | update | {JsonConvert.SerializeObject(dto) + " " + ErrorMessage}");
            }

        }

        public DataTable getByIdentifier(string identifier, string value)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;

            _params.AddWithValue("@identifier", identifier);
            _params.AddWithValue("@value", value);

            DataTable dt = this.ExecuteRead(@"dbo.sp_students_get", _params);

            return dt;
        }
        
    }
}