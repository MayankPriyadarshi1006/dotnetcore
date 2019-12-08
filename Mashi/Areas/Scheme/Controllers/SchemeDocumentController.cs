//using Mashi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mashi.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using Mashi.App_Code;

namespace Mashi.Areas.Scheme.Controllers
{
    public class SchemeDocumentController : Controller
    {
        //
        // GET: /Scheme/SchemeDocument/
        private MASHIEntities dbconnect = new MASHIEntities();
        DAL db = new DAL();
        public ActionResult Index()
        {
            return View();
        }

        #region SchemeDocuments Mayank

        public ActionResult SchemeDocuments()
        {
            if (Session["CUser"] != null)
            {
                UserModel currentuser = new UserModel();
                currentuser = (UserModel)Session["CUser"];
                ViewBag.SchemeTypeId = new SelectList(dbconnect.SchemeTypes.Where(x => x.IsEnabled == true), "SchemeTypeId", "SchemeTypeName");
            }

            else
            {
                Session["returnurl"] = Request.Url.ToString();
                return Redirect("~/Home/Index");
            }

            return View();
        }


        [HttpPost]
        public string SaveSchemeDetails(int SchemeTypeId, string SchemeDocumentName, string AbSchemeDocumentName, string Description, string AbDescription, int SchemeDocumentOrder, string AttachImagePathfilename, string AttachPdfPathfilename)
        {
            StringBuilder result = new StringBuilder();
            if (Session["CUser"] != null)
            {
                try
                {
                    UserModel currentuser = new UserModel();
                    currentuser = (UserModel)Session["CUser"];
                    SqlParameter[] parameter ={
                                     new SqlParameter("@intSchemeTypeId",SchemeTypeId),
                                     new SqlParameter("@chvnSchemeDocumentName",SchemeDocumentName),
                                     new SqlParameter("@chvnAbSchemeDocumentName",AbSchemeDocumentName),
                                     new SqlParameter("@chvnDescription",Description),         
                                     new SqlParameter("@chvnAbDescription",AbDescription),      
                                     new SqlParameter("@intSchemeDocumentOrder",SchemeDocumentOrder),
                                     new SqlParameter("@chvnImagePath",AttachImagePathfilename),
                                     new SqlParameter("@chvnPDFPath",AttachPdfPathfilename),
                                      new SqlParameter("@inbCreatedBy",currentuser.UserId),               
                                     };
                    long dr = db.SPExecuteScalar("[WebApplication_SP].[usp_SchemeDocument_Insert]", parameter);
                    if (dr > 0)
                    {
                        result.Append("1");

                    }
                    else
                    {
                        result.Append("0");
                    }

                }
                catch (Exception)
                {

                    result.Append("Error occurred while adding Scheme details.");
                }

            }
            else
            {
                result.Append("Session Expire");

            }
            return result.ToString();
        }

        public string GetSchemeList(Int64? SchemeTypeId,string SchemeDocumentName, string AbSchemeDocumentName, int PageNumber, int PageSize)
        {
            StringBuilder result = new StringBuilder();
            if (Session["CUser"] != null)
            {
                try
                {

             
                    DataSet ds = new DataSet();
                    SqlParameter[] parameters = 
                                   {           
                                          new SqlParameter("@inbSchemeId", SqlDbType.BigInt) { Value = SchemeTypeId },
                                         new SqlParameter("@chvnSchemeDocumentName", SqlDbType.NVarChar) { Value = SchemeDocumentName },
                                         new SqlParameter("@chvnAbSchemeDocumentName", SqlDbType.NVarChar) { Value = AbSchemeDocumentName },
                                         new SqlParameter("@intPageNumber", SqlDbType.BigInt) { Value = PageNumber },
                                         new SqlParameter("@intPageSize", SqlDbType.BigInt) { Value = PageSize },
                                 
                                   };

                    ds = db.SPExecuteDataset("[WebApplication_SP].[usp_SchemeDocumentList]", parameters, "DS");
                    result.Append("<table id ='ProductList' class='table table-striped table-bordered table-hover'>");
                    result.Append("<thead><tr>");
                    result.Append("<th>Scheme Type Name</th>");
                    result.Append("<th>Scheme Document Name</th>");
                    result.Append("<th>Ab Scheme Document Name</th>");
                    result.Append("<th>Description</th>");
                    result.Append("<th>Ab Description</th>");
                    result.Append("<th>Image Path</th>");
                    result.Append("<th>PDF Path</th>");
                    result.Append("<th>Scheme Document Order</th>");
                    result.Append("<th style='text-align:center'>Action</th>");
                    result.Append("</tr></thead><tbody>");
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string imag1 = "";
                                string ImagePath = dr["ImagePath"].ToString();
                                if (ImagePath != "")
                                {
                                    imag1 = (string)dr["ImagePath"];
                                }
                                result.Append("<tr>");
                                result.Append("<td>" + Convert.ToString(dr["SchemeTypeName"]) + "</td>");
                                result.Append("<td>" + Convert.ToString(dr["SchemeDocumentName"]) + "</td>");
                                result.Append("<td>" + Convert.ToString(dr["AbSchemeDocumentName"]) + "</td>");
                                result.Append("<td>" + Convert.ToString(dr["Description"]) + "</td>");
                                result.Append("<td>" + Convert.ToString(dr["AbDescription"]) + "</td>");
                                result.Append("<td><img height='75px' width='75px' src=" + imag1 + " " + (imag1) + "/> </td>");
                                //result.Append("<td>" + Convert.ToString(dr["PDFPath"]) + "</td>");
                                result.Append("<td><a class='button' href='" + Convert.ToString(dr["PDFPath"]) + "' target='_blank'>Download</a></td>");
                                result.Append("<td>" + Convert.ToString(dr["SchemeDocumentOrder"]) + "</td>");
                                result.Append("<td align='center'>");
                                result.Append("<button class='bg-orange btn' data-toggle='modal' data-target='.bs-example-modal-lg' type='button' onclick='EditSchemeDetails(\"" + dr["SchemeDocumentId"] + "\")'>Edit</button>");
                                result.Append("<button class='bg-orange btn'  type='button' onclick='DeleteSchemeRecord(\"" + dr["SchemeDocumentId"] + "\")'>Delete</button>");
                                result.Append("</td>");
                                result.Append("</tr>");
                            }
                            result.Append("</tbody></table>");
                            result.Append("~" + ds.Tables[1].Rows[0][0].ToString());
                        }
                        else
                        {
                            result.Append("<tr><td colspan=5>No Records Found!! </td></tr>");
                        }
                    }
                    else
                    {
                        result.Append("<tr><td colspan=5>No Records Found!! </td></tr>");
                    }
                }
                catch (Exception ex)
                {
                    result.Append("Error during geting List");
                }
            }

            else
            {
                return "Session Expired";
            }

            return result.ToString();
        }


        public string GetSchemeListById(int SchemeDocumentId)
        {
            StringBuilder result = new StringBuilder();
            if (Session["CUser"] != null)
            {
                try
                {
                    UserModel currentuser = new UserModel();
                    currentuser = (UserModel)Session["CUser"];
                    SqlParameter[] parameter ={
                                    new SqlParameter("@inbSchemeDocumentId",SchemeDocumentId)
                                    };
                    DataSet ds = db.SPExecuteDataset("[WebApplication_SP].[usp_SchemeDocumentById]", parameter, "DATASET");
                    if (ds != null)
                    {

                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                result.Append(dr["SchemeTypeId"] + "~" + dr["SchemeDocumentName"] + "~" + dr["AbSchemeDocumentName"] + "~" + dr["Description"] + "~" + dr["AbDescription"] + "~" + dr["ImagePath"] + "~" + dr["PDFPath"] + "~" + dr["SchemeDocumentOrder"]);

                            }

                        }
                    }

                }
                catch (Exception)
                {

                    result.Append("Error occurred while Fetching Module details.");
                }

            }
            else
            {
                result.Append("Session Expired");
            }

            return result.ToString();

        }

        public string DeleteSchemeRecord(int SchemeDocumentId)
        {

            StringBuilder result = new StringBuilder();
            if (Session["CUser"] != null)
            {
                try
                {
                    UserModel currentuser = new UserModel();
                    currentuser = (UserModel)Session["CUser"];
                    SqlParameter[] parameter ={
                                     new SqlParameter("@intSchemeDocumentId",SchemeDocumentId),                                                                   
                                     };
                    long dr = db.SPExecuteNonQuery("[WebApplication_SP].[usp_SchemeDocument_Delete _ById]", parameter);
                    if (dr > 0)
                    {
                        result.Append("1");

                    }
                    else
                    {
                        result.Append("0");
                    }

                }
                catch (Exception)
                {

                    result.Append("Error occurred while Deleting Scheme Document  details.");
                }

            }
            else
            {
                result.Append("Session Expire");

            }
            return result.ToString();
        }

        [HttpPost]
        public string UpdateSchemeRecord(Int64 SchemeDocumentId, int SchemeTypeId, string SchemeDocumentName, string AbSchemeDocumentName, string Description, string AbDescription, int SchemeDocumentOrder, string AttachImagePathfilename, string AttachPdfPathfilename)
        {

            StringBuilder result = new StringBuilder();
            if (Session["CUser"] != null)
            {
                try
                {
                    UserModel currentuser = new UserModel();
                    currentuser = (UserModel)Session["CUser"];
                    SqlParameter[] parameter ={
                                     new SqlParameter("@inbSchemeDocumentId",SchemeDocumentId),
                                     new SqlParameter("@intSchemeTypeId",SchemeTypeId),
                                     new SqlParameter("@chvnSchemeDocumentName",SchemeDocumentName),
                                     new SqlParameter("@chvnAbSchemeDocumentName",AbSchemeDocumentName),
                                     new SqlParameter("@chvnDescription",Description),         
                                     new SqlParameter("@chvnAbDescription",AbDescription),      
                                     new SqlParameter("@intSchemeDocumentOrder",SchemeDocumentOrder),
                                     new SqlParameter("@chvnImagePath",AttachImagePathfilename),
                                     new SqlParameter("@chvnPDFPath",AttachPdfPathfilename),
                                      new SqlParameter("@intModifiedBy",currentuser.UserId),
                             
                                     };
                    long dr = db.SPExecuteScalar("[WebApplication_SP].[usp_SchemeDocument_Update]", parameter);

                    if (dr == 1)
                    {
                        result.Append("1");
                    }
                    else
                    {

                        result.Append("0");
                    }
                }
                catch (Exception)
                {

                    result.Append("Error occurred while Updating Scheme details.");
                }

            }
            else
            {
                result.Append("Session Expire");

            }
            return result.ToString();
        }



        #endregion



    }
}
