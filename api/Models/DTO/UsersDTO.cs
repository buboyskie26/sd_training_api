using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace api.Models.DTO
{
    public class UsersSigninDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string password { get; set; }
    }
    public class UsersReturnDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UsersCreateDTO
    {

        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string email { get; set; }

    }

    public class UsersUpdateDTO
    {

        [Required(ErrorMessage = "Name is required.")]
        public string name { get; set; }
        [Required(ErrorMessage = "Is Active is required.")]
        public string is_active { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string email { get; set; }

    }

    public class ChangeStatusDTO
    {

        [Required(ErrorMessage = "Is Active is required.")]
        public string is_active { get; set; }

    }
    public class UserForgotPasswordDTO
    {

        [Required(ErrorMessage = "Email is required.")]
        public string email { get; set; }

    }

    public class ChangeStatusDto
    {
        public bool is_active { get; set; }

    }
}