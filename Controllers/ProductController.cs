using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using swensen_api.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Azure.Identity;
using System.Dynamic;
using swensen_api.Class;
using System.Globalization;
using System.Web;
using System.Runtime.InteropServices;

namespace swensen_api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController : ControllerBase
{
    private readonly DBContext _dbContext;

    public ProductController(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    bool _status = true;
    string _message = "";
    string _error = "";
    public Dictionary<string, dynamic> returnData = new Dictionary<string, dynamic>();

    public class ReqRemove
    {
        public string id { get; set; }
    }
    public class ReqList
    {
        public string cate_id { get; set; }
    }

    public class ReqUser
    {
        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string tel { get; set; }
        public string gender { get; set; }
        public string birth_date { get; set; }
    }

    public class ReqPro
    {
        public string name { get; set; }
        public string cage_id { get; set; }

        public List<IFormFile> image { get; set; }
    }

    public class image
    {
        public string cage_id { get; set; }
    }

    [HttpPost("list")]
    public ActionResult Detail([FromBody] ReqList req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            var resPro = _dbContext.Product.ToList();
            // if (!string.IsNullOrEmpty(req.cate_id))
            // {
            //     resPro = resPro.Where()
            // }

            // resPro = _dbContext.Product.ToList();
            if (resPro != null)
            {
                foreach (var item in resPro)
                {
                    list.Add(new
                    {
                        id = item.ID,
                        name = item.NAME,
                        file_image = $"data:image/{item.FILE_EXE.Replace(".", "")};base64," + Convert.ToBase64String(item.FILE_IMAGE),
                        recommend = item.RECOMMEND,
                        is_delete = item.IS_DELETE,
                        promotion = item.PROMOTION,
                        news = item.NEWS,
                        cate_id = item.CATE_ID,
                        cate_name = _dbContext.Category.Where(x => x.ID == item.CATE_ID).Select(x => x.NAME_TH).FirstOrDefault(),
                    });
                }

                returnData["data"] = list;
            }

        }
        catch (Exception ex)
        {
            _transection.Rollback();
            _status = false;
            _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
        }

        return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    }

    [HttpPost("submit")]
    public ActionResult Submit([FromForm] ReqPro req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (req.image.Count() > 0)
            {
                foreach (var img in req.image)
                {
                    if (img.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            // get binary of img
                            img.CopyTo(ms);

                            var fileBytes = ms.ToArray();

                            // string fileExt = MimeTypeMap.GetExtension(img.ContentType);
                            string fileExt = Path.GetExtension(img.FileName);

                            string fileId = Utilities.GetUIID();

                            // Console.WriteLine("audio/wav -> " + MimeTypeMap.GetExtension(img.ContentType));

                            var fileUpload = new ProductModel();

                            fileUpload.ID = fileId;
                            fileUpload.NAME = req.name;
                            fileUpload.FILE_IMAGE = fileBytes;
                            fileUpload.FILE_EXE = fileExt;
                            fileUpload.RECOMMEND = 0;
                            fileUpload.IS_ACTIVE = 1;
                            fileUpload.IS_DELETE = 0;
                            fileUpload.PROMOTION = 0;
                            fileUpload.NEWS = 0;
                            fileUpload.CATE_ID = req.cage_id;

                            _dbContext.Product.Add(fileUpload);
                            _dbContext.SaveChanges();
                        }
                    }
                }

                _transection.Commit();
                _status = true;
                _message = "";
            }
            else
            {
                _status = false;
                _message = "กรุณาแนบรูปภาพ";
            }

            // returnData["data"] = data;

        }
        catch (Exception ex)
        {
            _transection.Rollback();
            _status = false;
            _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
        }

        return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    }

    [HttpPost("remove")]
    public ActionResult Remove([FromBody] ReqRemove req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (!string.IsNullOrEmpty(req.id))
            {
                var res = _dbContext.Product.Where(x => x.ID == req.id).FirstOrDefault();

                if (res != null)
                {
                    _dbContext.Product.Remove(res);
                    _dbContext.SaveChanges();
                }

                _transection.Commit();
                _status = true;
                _message = "";
            }
            else
            {
                _status = false;
                _message = "กรุณาแนบรูปภาพ";
            }

            // returnData["data"] = data;

        }
        catch (Exception ex)
        {
            _transection.Rollback();
            _status = false;
            _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
        }

        return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    }



}
