using Mashi.App_Code;
using Mashi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mashi.Areas.Welcome.Controllers
{
    public class SetupController : Controller
    {
        private MASHIEntities db = new MASHIEntities();
       
        DAL dbconnect = new DAL();
        //
        // GET: /Welcome/Setup/

        [ChildActionOnly]
        public ActionResult AllMenu()
        {
            if (Session["CUser"] != null)
            {
                StringBuilder result = new StringBuilder();
                UserModel currentuser = new UserModel();
                currentuser = (UserModel)Session["CUser"];
                SqlParameter[] parameters =
                            {     
                             
                              new SqlParameter("@inbUserId", SqlDbType.BigInt) { Value = currentuser.UserId },
                             
                            };
                DataTable dt = new DataTable();
                dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_UserPermission_Select_List]", parameters, "permissions");

                #region
                var getUrl = (dynamic)null;
                if (currentuser != null)
                {

                    getUrl = (from role in db.Roles
                              join userrole in db.UserRoles
                              on role.RoleId equals userrole.RoleId
                              where  userrole.UserId == currentuser.UserId
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
                if (getUrl != null && getUrl.DefaultUrl != null && (getUrl.DefaultUrl).ToString() != "/Client/DashBoard")
                {
                    ViewBag.Dashboardurl = (getUrl.DefaultUrl).ToString();
                }
                else
                {
                    ViewBag.Dashboardurl = "/Client/DashBoard";
                }
                #endregion

                foreach (DataRow dr in dt.Rows)
                {

                    int PID = Convert.ToInt32(dr["ModuleId"]);
                    string ModuleName = Convert.ToString(dr["ModuleName"]);
                    string URL = Convert.ToString(dr["Url"]);
                    #region htmlClass
                    string HtmlClass = "";
                    if (URL == "/Client/DashBoard")
                    {
                        HtmlClass = "fa-tachometer";
                    }
                    else if (URL == "/Client/Tracking/DashBoard")
                    {
                        HtmlClass = "fa-map-marker";
                    }
                    else if (URL == "/Client/Meeting/Service")
                    {
                        HtmlClass = "fa-cogs";
                    }
                    else if (URL == "/Client/Order/DashBoard")
                    {
                        HtmlClass = "fa-area-chart";
                    }
                    else if (URL == "/Client/Visibility/DashBoard")
                    {
                        HtmlClass = "fa-bar-chart";
                    }
                    else if (URL == "/Client/HR/Index")
                    {
                        HtmlClass = "fa-female";
                    }
                    else if (URL == "/Client/setup/PersonalSettings")
                    {
                        HtmlClass = "fa-user";
                    }
                    else
                    {
                        HtmlClass = "fa-user";
                    }
                    #endregion
                    var UserRoleList = db.UserRoles.Where(x => x.UserId == currentuser.UserId).Select(y => y.RoleId).ToList();
                    var RMP = db.RoleModulePermissions.Where(x =>  UserRoleList.Contains(x.RoleId)).Select(x => x.ModuleId).ToList();


                    var ParentModule = db.Modules.Where(x => x.ModuleId == PID  && RMP.Contains(x.ModuleId)).FirstOrDefault();

                    if (ParentModule != null)
                    {
                        var subchilds = db.Modules.Where(x => x.ParentModuleId == ParentModule.ModuleId &&  RMP.Contains(x.ModuleId)).OrderBy(x => x.ModuleOrder).ToList();
                        if (subchilds.Count > 0)
                        {
                            result.Append("<li><a><i class='fa " + HtmlClass + " fa-fw'></i> " + ModuleName + " <span class='fa fa-chevron-down'></span></a>");
                            //SubChilds
                            result.Append("<ul class='nav child_menu'>");
                            foreach (var cmodule in subchilds)
                            {
                                var secondsubchilds = db.Modules.Where(x => x.ParentModuleId == cmodule.ModuleId  && RMP.Contains(x.ModuleId)).OrderBy(x => x.ModuleOrder).ToList();
                                if (secondsubchilds.Count > 0)
                                {
                                    result.Append("<li><a>" + cmodule.ModuleName + " <span class='fa fa-chevron-down'></span></a>");
                                    result.Append("<ul class='nav child_menu'>");
                                    foreach (var ssubchild in secondsubchilds)
                                    {
                                        result.Append("<li class='sub_menu'><a href='" + ssubchild.Url + "'>" + ssubchild.ModuleName + "</a></li>");
                                    }
                                    result.Append("</ul>");
                                    result.Append("</li>");
                                }
                                else
                                {
                                    result.Append("<li><a href='" + cmodule.Url + "'>" + cmodule.ModuleName + "</a></li>");
                                }
                            }
                            result.Append("</ul>");
                            //End
                            result.Append("</li>");
                        }
                        else
                        {
                            result.Append("<li><a href='" + ParentModule.Url + "'><i class='fa " + HtmlClass + " fa-fw'></i> " + ParentModule.ModuleName + " </a></li>");
                        }
                    }
                }
                ViewBag.Menu = result.ToString();
            }
            else
            {
                ViewBag.Menu = "";
            }

            return View();
        }

    }
}
