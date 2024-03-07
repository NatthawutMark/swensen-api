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

public class UsersController : ControllerBase
{
    private readonly DBContext _dbContext;

    public UsersController(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    bool _status = true;
    string _message = "";
    string _error = "";
    int _status_code = 200;
    public Dictionary<string, dynamic> returnData = new Dictionary<string, dynamic>();

    public class ReqData
    {
        public string id { get; set; }
    }

    public class ReqUser
    {
        public string username { get; set; }
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
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }

    [HttpPost("detail")]
    public ActionResult Detail([FromBody] ReqData req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            if (!string.IsNullOrEmpty(req.id))
            {
                var resUser = _dbContext.USERS.FirstOrDefault(x => x.ID == req.id);
                if (resUser != null)
                {
                    data.id = resUser.ID;
                    // data.user_name = resUser.re;
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

    [HttpPost("register")]
    public ActionResult Register([FromBody] ReqUser req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (!string.IsNullOrEmpty(req.email))
            {
                var resUser = _dbContext.USERS.FirstOrDefault(x => x.USERNAME == req.username);
                if (resUser != null)
                {
                    _status = false;
                    _message = "Username ซ้ำ กรุณาใช้อีเมลอื่น";
                }
                else
                {
                    var newUser = new Users();
                    newUser.ID = Utilities.GetUIID();
                    newUser.USERNAME = !string.IsNullOrEmpty(req.username) ? req.username : "";
                    newUser.EMAIL = !string.IsNullOrEmpty(req.email) ? req.email : "";
                    newUser.PASSWORD = !string.IsNullOrEmpty(req.password) ? req.password : "";
                    newUser.FIRST_NAME = !string.IsNullOrEmpty(req.first_name) ? req.first_name : "";
                    newUser.LAST_NAME = !string.IsNullOrEmpty(req.last_name) ? req.last_name : "";
                    newUser.TEL = !string.IsNullOrEmpty(req.tel) ? req.tel : "";
                    newUser.GENDER = !string.IsNullOrEmpty(req.gender) ? req.gender : "";
                    newUser.BIRTH_DATE = !string.IsNullOrEmpty(req.birth_date) ? DateTime.Parse(req.birth_date, CultureInfo.InvariantCulture) : DateTime.MinValue;
                    newUser.IS_ACTIVE = 1;
                    newUser.IS_DELETE = 0;
                    _dbContext.USERS.Add(newUser);
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

    [HttpPost("login")]
    public ActionResult Login([FromBody] ReqLogin req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (req.type == "admin")
            {
                var resAdmin = _dbContext.ADMIN.FirstOrDefault(x => x.USERNAME == req.username && x.PASSWORD == req.password);
                if (resAdmin != null)
                {
                    data.id = resAdmin.ID;
                    data.email = resAdmin.EMAIL;
                    data.username = resAdmin.USERNAME;
                    data.first_name = resAdmin.FIRST_NAME;
                    data.last_name = resAdmin.LAST_NAME;
                    data.tel = resAdmin.TEL;
                    data.type = "admin";
                }
                else
                {

                    _status = false;
                    _message = "ไม่พบบัญชีผู้ใช้ กรุณาลองอีกครั้ง";
                    _status_code = 200;
                }
            }
            else
            {
                var resUsers = _dbContext.USERS.FirstOrDefault(x => x.USERNAME == req.username && x.PASSWORD == req.password);
                if (resUsers != null)
                {
                    data.id = resUsers.ID;
                    data.email = resUsers.EMAIL;
                    data.username = resUsers.USERNAME;
                    data.first_name = resUsers.FIRST_NAME;
                    data.last_name = resUsers.LAST_NAME;
                    data.tel = resUsers.TEL;
                    data.gender = resUsers.GENDER;
                    data.birth_date = resUsers.BIRTH_DATE;
                    data.type = "user";
                }
                else
                {
                    _status = false;
                    _message = "ไม่พบบัญชีผู้ใช้ กรุณาลองอีกครั้ง";
                    _status_code = 200;
                }
            }


            returnData["data"] = data;
        }
        catch (Exception ex)
        {
            _transection.Rollback();

            _status_code = 500;
            _status = false;
            _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
        }

        return StatusCode(_status_code, new { status = _status, message = _message, error = _error, results = returnData });
    }


}
