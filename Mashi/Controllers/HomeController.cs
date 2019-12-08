using Mashi.App_Code;
using Mashi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mashi.Controllers
{
    public class HomeController : Controller
    {
        private MASHIEntities db = new MASHIEntities();
        DAL dbconnect = new DAL();
        #region Login 
        public ActionResult Index()
        {
            ViewBag.Message = "";
            string User_name = string.Empty;
            string Password = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                User_name = reqCookies["UserName"].ToString();
                Password = reqCookies["Password"].ToString();
                UserLoginModel model = new UserLoginModel();
                long loginstatus = model.CheckLogin(User_name, Password);
                if (loginstatus > 0)
                {
                    var user = db.Users.Where(x => x.UserName == User_name).FirstOrDefault();
                    UserModel cuser = new UserModel();
                    cuser.AbFullName = user.AbFullName;
                    cuser.EmailId = user.EmailId;
                    cuser.FullName = user.FullName;
                    cuser.IsEnabled = user.IsEnabled;
                    cuser.MobileNumber = user.MobileNumber;
                    cuser.OTP = user.OTP;
                    cuser.ProfilePhotoPath = user.ProfilePhotoPath;
                    cuser.UserId = user.UserId;
                    cuser.UserName = user.UserName;
                    cuser.UserType = user.UserType;
                    #region  URL
                    var getUrl = (dynamic)null;
                    if (cuser != null)
                    {
                        Session["CUser"] = cuser;
                        getUrl = (from role in db.Roles
                                  join userrole in db.UserRoles
                                  on role.RoleId equals userrole.RoleId
                                  where   userrole.UserId == cuser.UserId
                                  orderby userrole.CreateDate descending
                                  select new
                                  {
                                      RoleId = role.RoleId,
                                      RoleName = role.RoleName,
                                      Description = role.Description,
                                      CreateDate = userrole.CreateDate,
                                      DefaultUrl = role.DefaultUrl
                                  }).FirstOrDefault();
                    }
                    #endregion
                    if (cuser != null)
                    {
                        Session["CUser"] = cuser;
                        if (model.StayLogin == true)
                        {
                            HttpCookie userInfo = new HttpCookie("userInfo");
                            userInfo["UserName"] = model.UserName;
                            userInfo["Password"] = model.Password;
                            userInfo.Expires = DateTime.Now.AddMinutes(60);
                            Response.Cookies.Add(userInfo);

                        }
                        #region TopMenu
                        SqlParameter[] parameters =
                            {     
                           
                              new SqlParameter("@inbUserId", SqlDbType.BigInt) { Value = cuser.UserId },
                             
                            };
                        DataTable dt = new DataTable();
                        dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_UserPermission_Select_List]", parameters, "permissions");
                        List<TopMenu> topmenu = new List<TopMenu>();
                        try
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                TopMenu tm = new TopMenu();
                                tm.ModuleName = Convert.ToString(dr["ModuleName"]);
                                tm.URL = Convert.ToString(dr["Url"]);
                                topmenu.Add(tm);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        Session["TopMenu"] = topmenu;
                        #endregion
                    }


                    if (Session["returnurl"] != null)
                    {
                        return Redirect(Session["returnurl"].ToString());
                    }
                     else
                    {
                        // return RedirectToRoute("TrackingDashBoard");
                        return Redirect((getUrl.DefaultUrl).ToString());
                    }
                }
               
            }
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                    long loginstatus = model.CheckLogin(model.UserName, model.Password);
                  
                    if (loginstatus > 0)
                    {

                        var user = db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();
                        UserModel cuser = new UserModel();
                        cuser.AbFullName = user.AbFullName;
                        cuser.EmailId = user.EmailId;
                        cuser.FullName = user.FullName;
                        cuser.IsEnabled = user.IsEnabled;
                        cuser.MobileNumber = user.MobileNumber;
                        cuser.OTP = user.OTP;
                        cuser.ProfilePhotoPath = user.ProfilePhotoPath;
                        cuser.UserId = user.UserId;
                        cuser.UserName = user.UserName;
                        cuser.UserType = user.UserType;
                        #region  URL
                        var getUrl = (dynamic)null;
                        if (cuser != null)
                        {
                            Session["CUser"] = cuser;
                            getUrl = (from role in db.Roles
                                      join userrole in db.UserRoles
                                      on role.RoleId equals userrole.RoleId
                                      where userrole.UserId == cuser.UserId
                                      orderby userrole.CreateDate descending
                                      select new
                                      {
                                          RoleId = role.RoleId,
                                          RoleName = role.RoleName,
                                          Description = role.Description,
                                          CreateDate = userrole.CreateDate,
                                          DefaultUrl = role.DefaultUrl
                                      }).FirstOrDefault();
                        }
                        #endregion
                        if (cuser != null)
                        {
                            Session["CUser"] = cuser;
                            if (model.StayLogin == true)
                            {
                                HttpCookie userInfo = new HttpCookie("userInfo");
                                userInfo["UserName"] = model.UserName;
                                userInfo["Password"] = model.Password;
                                userInfo.Expires = DateTime.Now.AddMinutes(60);
                                Response.Cookies.Add(userInfo);

                            }
                            #region TopMenu
                            SqlParameter[] parameters =
                            {     
                           
                              new SqlParameter("@inbUserId", SqlDbType.BigInt) { Value = cuser.UserId },
                             
                            };
                            DataTable dt = new DataTable();
                            dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_UserPermission_Select_List]", parameters, "permissions");
                            List<TopMenu> topmenu = new List<TopMenu>();
                            try
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    TopMenu tm = new TopMenu();
                                    tm.ModuleName = Convert.ToString(dr["ModuleName"]);
                                    tm.URL = Convert.ToString(dr["Url"]);
                                    topmenu.Add(tm);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            Session["TopMenu"] = topmenu;
                            #endregion
                        }


                        if (Session["returnurl"] != null)
                        {
                            return Redirect(Session["returnurl"].ToString());
                        }
                        else
                        {
                            // return RedirectToRoute("TrackingDashBoard");
                            return Redirect((getUrl.DefaultUrl).ToString());
                        }
                        
                    }
                    else if (loginstatus == -2)
                    {
                        ModelState.AddModelError("", "Username or password is wrong.");
                    }
                    else
                    {
                     
                        ModelState.AddModelError("", "Username or password is wrong.");
                    }
                

            }
 

            return View(model);
        }

        #endregion       

        #region Logout
        public ActionResult LogOut()
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                userInfo.Expires = DateTime.Now.AddMinutes(-60);
                Response.Cookies.Add(userInfo);

            }
            Session.Abandon();
            return Redirect("~/Home/Index");
        }
        #endregion


        

    }
}
