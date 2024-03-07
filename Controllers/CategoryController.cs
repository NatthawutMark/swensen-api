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

public class CategoryController : ControllerBase
{
    private readonly DBContext _dbContext;

    public CategoryController(DBContext dbContext)
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
        public string name { get; set; }
        public string status { get; set; }
    }

    public class Req
    {
        public string? name { get; set; }
    }

    [HttpGet("")]
    public ActionResult List()
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            var resCate = _dbContext.CATEGORY.Where(x=>x.IS_DELETE == 0).ToList();
            if (resCate.Count() > 0)
            {
                foreach (var item in resCate)
                {
                    list.Add(new
                    {
                        id = item.ID,
                        name_th = item.NAME_TH,
                        name_en = item.NAME_EN,
                        is_active = item.IS_ACTIVE,
                        is_delete = item.IS_DELETE,
                    });
                }
            }

            returnData["data"] = list;
        }
        catch (Exception ex)
        {
            _transection.Rollback();
            _status = false;
            _message = ex.Message != null ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
        }

        return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    }

    // [HttpPost("")]
    // public ActionResult Add([FromBody] Req req)
    // {
    //     var _transection = _dbContext.Database.BeginTransaction();
    //     try
    //     {
    //         dynamic data = new ExpandoObject();
    //         List<dynamic> list = new List<dynamic>();
    //         // if (!string.IsNullOrEmpty(req.name))
    //         // {
    //         //     string name = req.name.Trim();
    //         //     var resCategory = _dbContext.Category.Where(x => x.ID == name).FirstOrDefault();
    //         // }

    //         returnData["data"] = list;

    //     }
    //     catch (Exception ex)
    //     {
    //         _transection.Rollback();
    //         _status = false;
    //         _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
    //         _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
    //     }

    //     return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    // }

    // [HttpGet(":id")]
    // public ActionResult Detail([FromBody] Req req)
    // {
    //     var _transection = _dbContext.Database.BeginTransaction();
    //     try
    //     {
    //         dynamic data = new ExpandoObject();
    //         List<dynamic> list = new List<dynamic>();
    //         returnData["data"] = list;

    //     }
    //     catch (Exception ex)
    //     {
    //         _transection.Rollback();
    //         _status = false;
    //         _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
    //         _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
    //     }

    //     return StatusCode(200, new { status = _status, message = _message, error = _error, results = returnData });
    // }

    [HttpGet("remove")]
    public ActionResult Remove(string id)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            if (!string.IsNullOrEmpty(id))
            {
                var res = _dbContext.CATEGORY.Where(x => x.ID == id).FirstOrDefault();
                if (res != null)
                {
                    res.IS_DELETE = 1;
                    _dbContext.CATEGORY.Update(res);
                    _dbContext.SaveChanges();
                    _transection.Commit();
                }

                _status = true;
                _message = "";
                _error = "";
            }
            else
            {
                _status = false;
                _message = "";
                _error = "";
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

    [HttpGet("active")]
    public ActionResult UpdateActive(string id)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            if (!string.IsNullOrEmpty(id))
            {
                var res = _dbContext.CATEGORY.Where(x => x.ID == id).FirstOrDefault();
                if (res != null)
                {
                    res.IS_ACTIVE = res.IS_ACTIVE == 1 ? 0 : 1;
                    _dbContext.CATEGORY.Update(res);
                    _dbContext.SaveChanges();
                    _transection.Commit();
                }

                _status = true;
                _message = "";
                _error = "";
            }
            else
            {
                _status = false;
                _message = "";
                _error = "";
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
}
