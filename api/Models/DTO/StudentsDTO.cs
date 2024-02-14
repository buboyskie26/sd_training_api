using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace api.Models.DTO
{
    public class StudentsReturnDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Course { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class StudentCreateDTO
    {

        [Required(ErrorMessage = "Firstname is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Middlename is required.")]
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Course is required.")]
        public string Course { get; set; }
        public bool IsActive { get; set; }
    }

}