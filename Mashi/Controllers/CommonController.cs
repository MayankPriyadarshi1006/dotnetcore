using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mashi.Models;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


namespace Mashi.Controllers
{
    public class CommonController : Controller
    {
        [HttpPost]
        public ContentResult UploadImages()
        {
            var r = new List<UploadFilesResult>();

            //foreach (string file in Request.Files)
            //{
            var Extensions = ConfigurationManager.AppSettings["imageextensions"].ToString().Split(',');
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            if (hpf.ContentLength == 0)
            {
                r.Add(new UploadFilesResult()
                {
                    Name = "",
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    Message = "Error during uploading file"
                });
            }
            else
            {

                if (Extensions.Contains(hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToLower()))
                {
                    string filename = GenerateUniqueName() + "." + hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToString();
                    string savedFileName = Path.Combine(Server.MapPath("~/UploadedImage"), Path.GetFileName(filename));
                    hpf.SaveAs(savedFileName); // Save the file

                    string url = ConfigurationManager.AppSettings["url"].ToString() + "UploadedImage/" + filename;
                    r.Add(new UploadFilesResult()
                    {
                        Name = url,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Message = "Success"
                    });
                }

                else
                {
                    r.Add(new UploadFilesResult()
                    {
                        Name = "",
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Message = "Allowed file extensions are " + ConfigurationManager.AppSettings["imageextensions"].ToString()
                    });
                }
            }


            // }

            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"message\":\"" + r[0].Message + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        [HttpPost]
        public ContentResult UploadAllFiles()
        {
            var r = new List<UploadFilesResult>();

            // double largeimagesize = Convert.ToInt32(ConfigurationManager.AppSettings["largefilesize"]);
            var Extensions = ConfigurationManager.AppSettings["fileextensions"].ToString().Split(',');
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;

            var postedfile = Request.Files[0];
            Stream strm = postedfile.InputStream;

            if (hpf.ContentLength == 0)
            {
                r.Add(new UploadFilesResult()
                {
                    Name = "",
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    Message = "Error during uploading file"
                });
            }
            else
            {
                if (Extensions.Contains(hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToLower()))
                {
                    // if (hpf.ContentLength <= largeimagesize * 1000)
                    if (1 == 1)
                    {
                        string filename = GenerateUniqueName() + "." + hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToString();
                        string savedFileName = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(filename));                     

                        hpf.SaveAs(savedFileName); // Save the file

                        string url = ConfigurationManager.AppSettings["url"].ToString() + "UploadedFiles/" + filename;
                        r.Add(new UploadFilesResult()
                        {
                            Name = url,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            Message = "Success"
                        });
                    }
                    else
                    {
                        r.Add(new UploadFilesResult()
                        {
                            Name = "",
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            Message = "Maximum size limit of uploading image is :4 MB"
                        });
                    }
                }
                else
                {
                    r.Add(new UploadFilesResult()
                    {
                        Name = "",
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Message = "Allowed file extensions are " + ConfigurationManager.AppSettings["fileextensions"].ToString()
                    });
                }
            }
            // }

            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"message\":\"" + r[0].Message + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        [HttpPost]
        public ContentResult UploadPdf()
        {
            var r = new List<UploadFilesResult>();

            // double largeimagesize = Convert.ToInt32(ConfigurationManager.AppSettings["largefilesize"]);
            var Extensions = ConfigurationManager.AppSettings["pdfextensions"].ToString().Split(',');
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;

            var postedfile = Request.Files[0];
            Stream strm = postedfile.InputStream;

            if (hpf.ContentLength == 0)
            {
                r.Add(new UploadFilesResult()
                {
                    Name = "",
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    Message = "Error during uploading file"
                });
            }
            else
            {
                if (Extensions.Contains(hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToLower()))
                {
                    // if (hpf.ContentLength <= largeimagesize * 1000)
                    if (1 == 1)
                    {
                        string filename = GenerateUniqueName() + "." + hpf.FileName.Split('.')[hpf.FileName.Split('.').Length - 1].ToString();
                        string savedFileName = Path.Combine(Server.MapPath("~/UploadedPDF"), Path.GetFileName(filename));

                        hpf.SaveAs(savedFileName); // Save the file

                        string url = ConfigurationManager.AppSettings["url"].ToString() + "UploadedPDF/" + filename;
                        r.Add(new UploadFilesResult()
                        {
                            Name = url,
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            Message = "Success"
                        });
                    }
                    else
                    {
                        r.Add(new UploadFilesResult()
                        {
                            Name = "",
                            Length = hpf.ContentLength,
                            Type = hpf.ContentType,
                            Message = "Maximum size limit of uploading image is :4 MB"
                        });
                    }
                }
                else
                {
                    r.Add(new UploadFilesResult()
                    {
                        Name = "",
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType,
                        Message = "Allowed file extensions are " + ConfigurationManager.AppSettings["pdfextensions"].ToString()
                    });
                }
            }
            // }

            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"message\":\"" + r[0].Message + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        public string GenerateUniqueName()
        {
            if (Session["CUser"] != null)
            {
                UserModel currentuser = new UserModel();
                currentuser = (UserModel)Session["CUser"];
                return currentuser.UserId.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            }
            else
            {
                return DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            }
        }

    }

    public class UploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}

