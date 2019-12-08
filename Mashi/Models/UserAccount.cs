using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using FluentValidation.Attributes;
using Mashi.App_Code;
using System.ComponentModel;

namespace Mashi.Models
{
    [Validator(typeof(ChangePassowrdValidator))]
    public class LocalPasswordModel
    {
        // [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]

        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool StayLogin { get; set; }

        public long CheckLogin(string username, string password)
        {
            try
            {
                DAL dbconnect = new DAL();
                SHAEncrypt encrpt = new SHAEncrypt();
                long result = 0;
                SqlParameter[] parameters =
                            {     //new SqlParameter("@intClientId", SqlDbType.Int) { Value = 0 },
                              new SqlParameter("@chvnLoginId", SqlDbType.VarChar, 512) { Value = username },
                              new SqlParameter("@chvnPassword", SqlDbType.VarChar, 200) { Value = password },
                              new SqlParameter("@inbUserId", SqlDbType.BigInt) { Direction=ParameterDirection.Output },
                            };
                result = SPExecuteScalar("[WebApplication_SP].[usp_User_Login_ById]", parameters);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public long CheckPreLogin(string username, string password)
        {
            try
            {
                DAL dbconnect = new DAL();
                SHAEncrypt encrpt = new SHAEncrypt();
                long result = 0;
                SqlParameter[] parameters =
                            {     //new SqlParameter("@intClientId", SqlDbType.Int) { Value = 0 },
                              new SqlParameter("@chvnLoginId", SqlDbType.VarChar, 512) { Value = username },
                              new SqlParameter("@chvnPassword", SqlDbType.VarChar, 200) { Value = password },
                              new SqlParameter("@inbUserId", SqlDbType.BigInt) { Direction=ParameterDirection.Output },
                            };
                DataTable dt = new DataTable();
                dt = dbconnect.SPExecuteDataTable("[MobileApplication_SP].[usp_User_Login_ForPreuser]", parameters, "prelogindata");
                if (dt.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    result = -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        #region get Child Users
        public List<long> GetAllChilds(long ParentUserId)
        {
            List<long> UserIds = new List<long>();
            DataTable dt = new DataTable();
            DAL dbconnect = new DAL();

            SqlParameter[] parameters =
                            {     //new SqlParameter("@intClientId", SqlDbType.Int) { Value = 0 },
                              new SqlParameter("@inbParentUserId", SqlDbType.BigInt, 512) { Value = ParentUserId },
                            };
            dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_ChildUsersByUserid_All]", parameters, "allchilds");
            foreach (DataRow dr in dt.Rows)
            {
                UserIds.Add(Convert.ToInt32(dr[0]));
            }
            return UserIds;
        }


        #endregion

        #region get Parents User
        public List<long> GetAllParents(long ChildUserId)
        {
            List<long> UserIds = new List<long>();
            DataTable dt = new DataTable();
            DAL dbconnect = new DAL();

            SqlParameter[] parameters =
                            {     
                              new SqlParameter("@inbChildUserId", SqlDbType.BigInt, 512) { Value = ChildUserId },
                            };
            dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_GetLevels_All]", parameters, "allParents");
            foreach (DataRow dr in dt.Rows)
            {
                UserIds.Add(Convert.ToInt32(dr[0]));
            }
            return UserIds;
        }


        #endregion

        #region Helper
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
        }
        #endregion
        #region ColorList
        public List<string> Colorlist()
        {
            List<string> Color = new List<string>();

            Color.Add("#00aaa0");
            Color.Add("#ff7a5a");
            Color.Add("#ffb85f");
            Color.Add("#462066");
            Color.Add("#fff");
            Color.Add("#F0F8FF");
            Color.Add("#FAEBD7");
            Color.Add("#00FFFF");
            Color.Add("#7FFFD4");
            Color.Add("#7FFFD4");
            Color.Add("#F0FFFF");
            Color.Add("#F5F5DC");
            Color.Add("#FFE4C4");
            Color.Add("#000000");
            Color.Add("#FFEBCD");
            Color.Add("#0000FF");
            Color.Add("#8A2BE2");
            Color.Add("#A52A2A");
            Color.Add("#DEB887");
            Color.Add("#5F9EA0");
            return Color;
        }
        #endregion
    }

    public class VerifyOTP// At time of login
    {
        [Required]
        [Display(Name = "Enter OTP")]
        public string EnteredOTP { get; set; }
        public string MobileNumber { get; set; }
        public int Userid { get; set; }
        public string SentOTP { get; set; }
        public bool IsValidMobileNumber { get; set; }


    }

    public class OTPModel//For After Login
    {
        [Required]
        [Display(Name = "Enter OTP")]
        public string EnteredOTP { get; set; }
        public string MobileNumber { get; set; }
        public long Userid { get; set; }
        public string SentOTP { get; set; }
        public bool IsValidMobileNumber { get; set; }


    }
    public class TopMenu
    {
        public string ModuleName { get; set; }
        public string URL { get; set; }

    }
    public class LeftMenu
    {
        public string ModuleName { get; set; }
        public string URL { get; set; }
        public string ParentModuleId { get; set; }

    }


    public class ChangePassowrdValidator : AbstractValidator<LocalPasswordModel>
    {
        public ChangePassowrdValidator()
        {

            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old Password is required.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New Password is required.");
            RuleFor(x => x.NewPassword).NotEqual(y => y.OldPassword).WithMessage("New password is same as old password.");

        }
    }

    public class UserModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public bool IsEnabled { get; set; }
        public string ProfilePhotoPath { get; set; }
        public string FullName { get; set; }
        public string AbFullName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string UserType { get; set; }
        public string OTP { get; set; }

    }
}