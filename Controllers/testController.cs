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

public class testController : ControllerBase
{
    private readonly DBContext _dbContext;

    public testController(DBContext dbContext)
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

    [HttpGet()]
    public ActionResult getGUID()
    {
        List<string> str = new List<string>();
        for (int i = 1; i <= 20; i++)
        {
            str.Add(Utilities.GetUIID());
        }
        return StatusCode(200, new { status = true, ressult = str });
    }
}
