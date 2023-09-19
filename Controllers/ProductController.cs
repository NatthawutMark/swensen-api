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

    public class ReqData
    {
        public string id { get; set; }
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

    public class ReqLogin
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    [HttpPost("list")]
    public ActionResult Detail([FromBody] ReqData req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            if (!string.IsNullOrEmpty(req.id))
            {
                var resProduct = _dbContext.Product.ToList();
                if (resProduct.Count > 0)
                {
                    foreach (var item in resProduct)
                    {
                        list.Add(new
                        {
                            id = item.ID,
                            name = item.NAME,
                            file_image = item.FILE_IMAGE,
                            recommend = item.RECOMMEND,
                            is_delete = item.IS_DELETE,
                            promotion = item.PROMOTION,
                            news = item.NEWS,
                            cate_id = item.CATE_ID,
                        });
                    }
                }

                returnData["data"] = data;
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
    public ActionResult Register([FromBody] ReqUser req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (!string.IsNullOrEmpty(req.email))
            {
                var resUser = _dbContext.Users.FirstOrDefault(x => x.EMAIL == req.email);
                if (resUser != null)
                {
                    _status = false;
                    _message = "Email ซ้ำ กรุณาใช้อีเมลอื่น";
                }
                else
                {
                    var newUser = new Users();
                    newUser.ID = Utilities.GetUIID();
                    newUser.EMAIL = !string.IsNullOrEmpty(req.email) ? req.email : "";
                    newUser.PASSWORD = !string.IsNullOrEmpty(req.password) ? req.password : "";
                    newUser.FIRST_NAME = !string.IsNullOrEmpty(req.first_name) ? req.first_name : "";
                    newUser.LAST_NAME = !string.IsNullOrEmpty(req.last_name) ? req.last_name : "";
                    newUser.TEL = !string.IsNullOrEmpty(req.tel) ? req.tel : "";
                    newUser.GENDER = !string.IsNullOrEmpty(req.gender) ? req.gender : "";
                    newUser.BIRTH_DATE = !string.IsNullOrEmpty(req.birth_date) ? DateTime.Parse(req.birth_date, CultureInfo.InvariantCulture) : null;
                    newUser.IS_ACTIVE = 1;
                    newUser.IS_DELETE = 0;
                    _dbContext.Users.Add(newUser);
                    _dbContext.SaveChanges();

                    _transection.Commit();
                    _status = true;
                }
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
