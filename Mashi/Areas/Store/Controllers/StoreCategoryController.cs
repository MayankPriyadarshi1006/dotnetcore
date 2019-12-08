using Mashi.App_Code;
using Mashi.Areas.Store.Models.StoreCategory;
using Mashi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mashi.Areas.Store.Controllers
{
    public class StoreCategoryController : Controller
    {
        DAL dbconnect = new DAL();
        private MASHIEntities db = new MASHIEntities();
        //
        // GET: /Store/StoreCategory/
        public ActionResult Index()
        {
            SqlParameter[] parameters = {
                                    new SqlParameter("@chvOperationType", SqlDbType.VarChar) { Value = "SELECT" }
                            };

            var dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_StoreCategory_Select_Delete_Update_Insert_ById]", parameters, "StoreCategory");

            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.DataRecords = dt.AsEnumerable();
            }

            return View();
        }

        public ActionResult AddStoreCategory(int? storeCategoryId)
        {
            UserModel currentuser = new UserModel();
            currentuser = (UserModel)Session["CUser"];

            StoreCategoryModel scm = new StoreCategoryModel();

            if (storeCategoryId != null)
            {
                SqlParameter[] parameters = {
                                    new SqlParameter("@intStoreCategoryId", SqlDbType.Int) { Value = storeCategoryId },
                                    new SqlParameter("@chvOperationType", SqlDbType.VarChar) { Value = "SELECT" }
                            };

                var dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_StoreCategory_Select_Delete_Update_Insert_ById]", parameters, "StoreCategory");

                DataRow dr = dt.Rows[0];
                scm.storeCategoryId = Convert.ToInt32(dr["StoreCategoryId"]);
                scm.storeCategoryCode = dr["StoreCategoryCode"].ToString();
                scm.storeCategoryName = dr["StoreCategoryName"].ToString();
                scm.storeCategoryNameAB = dr["AbStoreCategoryName"].ToString();

                if (!(string.IsNullOrEmpty(dr["ParentStoreCategoryId"].ToString())))
                {
                    scm.ParentStoreCategoryId = Convert.ToInt32(dr["ParentStoreCategoryId"]);
                }
            }

            ViewBag.ParentDD = new SelectList(db.StoreCategories.Where(x => x.IsDeleted == false && x.ParentStoreCategoryId == null), "StoreCategoryId", "StoreCategoryCode");

            return View(scm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStoreCategory(StoreCategoryModel model)
        {
            if (Session["CUser"] != null)
            {
                if (ModelState.IsValid)
                {
                    UserModel currentuser = new UserModel();
                    currentuser = (UserModel)Session["CUser"];

                    if (model.ParentStoreCategoryId != null)
                    {
                        SqlParameter[] parameters = {
                                    new SqlParameter("@inbCurrentUserId", SqlDbType.BigInt) { Value = currentuser.UserId },
                                    new SqlParameter("@intStoreCategoryId", SqlDbType.Int) { Value = model.storeCategoryId },
                                    new SqlParameter("@chvnStoreCategoryCode", SqlDbType.NVarChar) { Value = model.storeCategoryCode },
                                    new SqlParameter("@chvnStoreCategoryName", SqlDbType.NVarChar) { Value = model.storeCategoryName },
                                    new SqlParameter("@chvnAbStoreCategoryName", SqlDbType.NVarChar) { Value = model.storeCategoryNameAB },
                                    new SqlParameter("@intParentStoreCategoryId", SqlDbType.Int) { Value = model.ParentStoreCategoryId },
                                    new SqlParameter("@chvOperationType", SqlDbType.VarChar) { Value = "UPDATE" }
                            };

                        var dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_StoreCategory_Select_Delete_Update_Insert_ById]", parameters, "StoreCategory");
                    }
                    else
                    {
                        SqlParameter[] parameters = {
                                    new SqlParameter("@inbCurrentUserId", SqlDbType.BigInt) { Value = currentuser.UserId },
                                    new SqlParameter("@chvnStoreCategoryCode", SqlDbType.NVarChar) { Value = model.storeCategoryCode },
                                    new SqlParameter("@chvnStoreCategoryName", SqlDbType.NVarChar) { Value = model.storeCategoryName },
                                    new SqlParameter("@chvnAbStoreCategoryName", SqlDbType.NVarChar) { Value = model.storeCategoryNameAB },
                                    new SqlParameter("@intParentStoreCategoryId", SqlDbType.Int) { Value = model.ParentStoreCategoryId },
                                    new SqlParameter("@chvOperationType", SqlDbType.VarChar) { Value = "INSERT" }
                            };

                        var dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_StoreCategory_Select_Delete_Update_Insert_ById]", parameters, "StoreCategory");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Required fields cannot be left blank");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Session Expired");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DeleteStoreCategory(int? StoreCategoryId)
        {
            if (Session["CUser"] != null)
            {
                UserModel currentuser = new UserModel();
                currentuser = (UserModel)Session["CUser"];

                SqlParameter[] parameters = {
                                    new SqlParameter("@intStoreCategoryId", SqlDbType.Int) { Value = StoreCategoryId },
                                    new SqlParameter("@inbCurrentUserId", SqlDbType.BigInt) { Value = currentuser.UserId },
                                    new SqlParameter("@chvOperationType", SqlDbType.VarChar) { Value = "DELETE" }
                            };

                var dt = dbconnect.SPExecuteDataTable("[WebApplication_SP].[usp_StoreCategory_Select_Delete_Update_Insert_ById]", parameters, "StoreCategory");

                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }
    }
}
