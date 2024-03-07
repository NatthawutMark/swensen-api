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
using Microsoft.IdentityModel.Tokens;

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

    int _code = 200;
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
        public string id { get; set; }
        public string action { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string qty { get; set; }
        public string cate_id { get; set; }
        public string special { get; set; }
        public List<Images> imageList { get; set; }
        public List<IFormFile> image { get; set; }
    }

    public class Images
    {
        public string id { get; set; }
        public string file_name { get; set; }
        public string file_size { get; set; }
        public string file_ext { get; set; }
        public List<IFormFile> file_blob { get; set; }
        // public string file_blob { get; set; }
        public string base64 { get; set; }

    }

    [HttpGet("special")]
    public ActionResult special()
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            var resPro = _dbContext.PRODUCT.Where(x => x.IS_ACTIVE == 1 && x.IS_DELETE == 0).OrderBy(x => x.CREATED_AT).ToList();

            var statusList = _dbContext.STATUS.ToList();
            if (resPro != null)
            {
                foreach (var item in resPro)
                {
                    var resStatus = statusList.Where(x => x.ID == item.STATUS_ID).FirstOrDefault();
                    var resProImg = _dbContext.PRODUCT_IMAGE.Where(x => x.PRODUCCT_ID == item.ID).OrderByDescending(x => x.UPDATED_AT).FirstOrDefault();
                    list.Add(new
                    {
                        id = item.ID,
                        cate_id = item.CATE_ID,
                        cate_name = _dbContext.CATEGORY.Where(x => x.ID == item.CATE_ID).Select(x => x.NAME_TH).FirstOrDefault(),
                        name = item.NAME,
                        price = item.PRICE,
                        qty = item.QTY,
                        status_id = item.STATUS_ID,
                        status_name = resStatus != null ? resStatus.NAME : "-",
                        news = item.NEWS,
                        promotion = item.PROMOTION,
                        recommend = item.RECOMMEND,
                        file_image = resProImg != null ? $"data:image/{resProImg.FILE_EXT.Replace(".", "")};base64,{Convert.ToBase64String(resProImg.FILE_BLOB)}" : "-",
                        is_active = item.IS_ACTIVE,
                        is_delete = item.IS_DELETE,
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

    [HttpPost("list")]
    public ActionResult List([FromBody] ReqList req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> list = new List<dynamic>();
            var resPro = _dbContext.PRODUCT.Where(x => x.IS_ACTIVE == 1 && x.IS_DELETE == 0).OrderBy(x => x.CREATED_AT).ToList();

            var statusList = _dbContext.STATUS.ToList();
            if (resPro != null)
            {
                foreach (var item in resPro)
                {
                    var resStatus = statusList.Where(x => x.ID == item.STATUS_ID).FirstOrDefault();
                    var resProImg = _dbContext.PRODUCT_IMAGE.Where(x => x.PRODUCCT_ID == item.ID).OrderByDescending(x => x.UPDATED_AT).FirstOrDefault();
                    list.Add(new
                    {
                        id = item.ID,
                        cate_id = item.CATE_ID,
                        cate_name = _dbContext.CATEGORY.Where(x => x.ID == item.CATE_ID).Select(x => x.NAME_TH).FirstOrDefault(),
                        name = item.NAME,
                        price = item.PRICE,
                        qty = item.QTY,
                        status_id = item.STATUS_ID,
                        status_name = resStatus != null ? resStatus.NAME : "-",
                        news = item.NEWS,
                        promotion = item.PROMOTION,
                        recommend = item.RECOMMEND,
                        file_image = resProImg != null ? $"data:image/{resProImg.FILE_EXT.Replace(".", "")};base64,{Convert.ToBase64String(resProImg.FILE_BLOB)}" : "-",
                        is_active = item.IS_ACTIVE,
                        is_delete = item.IS_DELETE,
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
            string status = "";
            var resPro = _dbContext.PRODUCT.ToList();
            if (!string.IsNullOrEmpty(req.qty) && decimal.Parse(req.qty) != 0)
            {
                status = "IN_STOCK";
            }
            else
            {
                status = "OUT_OF_STOCK";
            }
            if (req.action == "add")
            {
                if (!string.IsNullOrEmpty(req.name))
                {
                    resPro = resPro.Where(x => x.NAME == req.name).ToList();
                }

                if (!string.IsNullOrEmpty(req.cate_id))
                {
                    resPro = resPro.Where(x => x.CATE_ID == req.cate_id).ToList();
                }

                if (resPro.Count() == 0)
                {
                    if (req.image.Count() > 0)
                    {
                        string idProduct = Utilities.GetUIID();
                        var newProduct = new ProductModel();
                        newProduct.ID = idProduct;
                        newProduct.CATE_ID = req.cate_id;
                        newProduct.NAME = req.name;
                        newProduct.PRICE = !string.IsNullOrEmpty(req.price) ? decimal.Parse(req.price) : 0;
                        newProduct.QTY = !string.IsNullOrEmpty(req.qty) ? int.Parse(req.qty) : 0;
                        newProduct.STATUS_ID = status;
                        switch (req.special)
                        {
                            case "news": newProduct.NEWS = 1; break;
                            case "promo": newProduct.PROMOTION = 1; break;
                            case "recom": newProduct.RECOMMEND = 1; break;
                            default: break;
                        }
                        newProduct.IS_ACTIVE = 1;
                        newProduct.IS_DELETE = 0;
                        newProduct.CREATED_AT = DateTime.Now;
                        newProduct.UPDATED_AT = DateTime.Now;

                        _dbContext.PRODUCT.Add(newProduct);
                        _dbContext.SaveChanges();

                        List<ProductImageModel> listImage = new List<ProductImageModel>();
                        foreach (var img in req.image)
                        {
                            if (img.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    // get binary of img
                                    img.CopyTo(ms);

                                    var fileBytes = ms.ToArray();
                                    string fileExt = Path.GetExtension(img.FileName);
                                    var fileSize = img.Length;
                                    string fileId = Utilities.GetUIID();

                                    listImage.Add(new ProductImageModel
                                    {
                                        ID = Utilities.GetUIID(),
                                        PRODUCCT_ID = idProduct,
                                        FILE_NAME = Path.GetFileName(img.FileName),
                                        FILE_EXT = fileExt,
                                        FILE_BLOB = fileBytes,
                                        FILE_SIZE = img.Length,
                                        IS_ACTIVE = 1,
                                        IS_DELETE = 0,
                                        CREATED_AT = DateTime.Now,
                                        UPDATED_AT = DateTime.Now
                                    });
                                }
                            }
                        }

                        if (listImage.Count() > 0)
                        {
                            _dbContext.PRODUCT_IMAGE.AddRange(listImage);
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
                }
                else
                {
                    _status = false;
                    _message = "สินค้านี้มีอยู่ กรุณาลองใหม่";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(req.id))
                {
                    var resProImg = _dbContext.PRODUCT_IMAGE.Where(x => x.PRODUCCT_ID == req.id).ToList();
                    if (resProImg.Count() > 0)
                    {
                        _dbContext.PRODUCT_IMAGE.RemoveRange(resProImg);
                        _dbContext.SaveChanges();
                    }
                    var resProUpdate = resPro.Where(x => x.ID == req.id).FirstOrDefault();
                    if (resProUpdate != null)
                    {
                        resProUpdate.NEWS = 0;
                        resProUpdate.PROMOTION = 0;
                        resProUpdate.RECOMMEND = 0;

                        resProUpdate.CATE_ID = req.cate_id;
                        resProUpdate.NAME = req.name;
                        resProUpdate.PRICE = !string.IsNullOrEmpty(req.price) ? decimal.Parse(req.price) : 0;
                        resProUpdate.QTY = !string.IsNullOrEmpty(req.qty) ? int.Parse(req.qty) : 0;
                        resProUpdate.STATUS_ID = status;
                        switch (req.special)
                        {
                            case "news": resProUpdate.NEWS = 1; break;
                            case "promo": resProUpdate.PROMOTION = 1; break;
                            case "recom": resProUpdate.RECOMMEND = 1; break;
                            default: break;
                        }
                        resProUpdate.UPDATED_AT = DateTime.Now;

                        _dbContext.PRODUCT.Update(resProUpdate);
                        _dbContext.SaveChanges();

                        List<ProductImageModel> listImage = new List<ProductImageModel>();
                        foreach (var img in req.image)
                        {
                            if (img.Length > 0)
                            {
                                using (var ms = new MemoryStream())
                                {
                                    // get binary of img
                                    img.CopyTo(ms);

                                    var fileBytes = ms.ToArray();
                                    string fileExt = Path.GetExtension(img.FileName);
                                    var fileSize = img.Length;
                                    string fileId = Utilities.GetUIID();

                                    listImage.Add(new ProductImageModel
                                    {
                                        ID = Utilities.GetUIID(),
                                        PRODUCCT_ID = req.id,
                                        FILE_NAME = Path.GetFileName(img.FileName),
                                        FILE_EXT = fileExt,
                                        FILE_BLOB = fileBytes,
                                        FILE_SIZE = img.Length,
                                        IS_ACTIVE = 1,
                                        IS_DELETE = 0,
                                        CREATED_AT = DateTime.Now,
                                        UPDATED_AT = DateTime.Now
                                    });
                                }
                            }
                        }

                        if (listImage.Count() > 0)
                        {
                            _dbContext.PRODUCT_IMAGE.AddRange(listImage);
                            _dbContext.SaveChanges();
                        }

                        _transection.Commit();
                        _status = true;
                        _message = "";
                    }
                    else
                    {
                        _status = false;
                        _message = "ไม่พบสินค้า";
                    }
                }
                else
                {
                    _status = false;
                    _message = "ไม่มี ID";
                }
            }

            // returnData["data"] = data;

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

    [HttpPost("remove")]
    public ActionResult Remove([FromBody] ReqRemove req)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();

            if (!string.IsNullOrEmpty(req.id))
            {
                var res = _dbContext.PRODUCT.Where(x => x.ID == req.id).FirstOrDefault();

                if (res != null)
                {
                    res.IS_DELETE = 1;
                    res.UPDATED_AT = DateTime.Now;
                    _dbContext.SaveChanges();
                }

                _transection.Commit();
                _status = true;
                _message = "";
            }
            else
            {
                _status = false;
                _message = "ไม่พบข้อมูล";
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

    [HttpGet("")]
    public ActionResult Detail(string id)
    {
        var _transection = _dbContext.Database.BeginTransaction();
        try
        {
            dynamic data = new ExpandoObject();
            List<dynamic> listImage = new List<dynamic>();

            if (!string.IsNullOrEmpty(id))
            {
                var res = (from pro in _dbContext.PRODUCT
                               // join proImg in _dbContext.PRODUCT_IMAGE on pro.ID equals proImg.PRODUCCT_ID
                           join resStatus in _dbContext.STATUS on pro.STATUS_ID equals resStatus.ID
                           join resCate in _dbContext.CATEGORY on pro.CATE_ID equals resCate.ID
                           where pro.ID == id
                           select new
                           {
                               pro = pro,
                               // proImg = proImg,
                               status = resStatus,
                               cate = resCate
                           }).FirstOrDefault();


                if (res != null)
                {
                    string strSpecial = "";
                    if (res.pro.PROMOTION == 1)
                        strSpecial = "pro";
                    if (res.pro.RECOMMEND == 1)
                        strSpecial = "recom";
                    if (res.pro.NEWS == 1)
                        strSpecial = "news";

                    var resProImg = _dbContext.PRODUCT_IMAGE.Where(x => x.PRODUCCT_ID == res.pro.ID).ToList();
                    if (resProImg.Count() > 0)
                    {
                        foreach (var item in resProImg)
                        {
                            listImage.Add(new
                            {
                                id = item.ID,
                                product_id = item.PRODUCCT_ID,
                                file_name = item.FILE_NAME,
                                file_blob = $"data:image/{item.FILE_EXT.Replace(".", "")};base64," + Convert.ToBase64String(item.FILE_BLOB),
                                file_ext = item.FILE_EXT,
                                file_size = item.FILE_SIZE.ToString(),
                            });

                        }
                    }

                    data.id = res.pro.ID;
                    data.cate_id = res.pro.CATE_ID;
                    data.cate_name = res.cate.NAME_TH;
                    data.name = res.pro.NAME;
                    data.price = res.pro.PRICE.ToString();
                    data.qty = res.pro.QTY.ToString();
                    data.status_id = res.pro.STATUS_ID;
                    data.status_name = res.status.NAME;
                    data.promotion = res.pro.PROMOTION;
                    data.recommend = res.pro.RECOMMEND;
                    data.news = res.pro.NEWS;
                    data.images = listImage;
                    data.special = strSpecial;
                }

                returnData["data"] = data;
                _status = true;
                _message = "";
            }
            else
            {
                _status = false;
                _message = "ไม่พบข้อมูล";
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

    [HttpGet("image/{id}")]
    public IActionResult GetFile(string id)
    {
        try
        {
            var fileEntity = _dbContext.PRODUCT_IMAGE.Where(file => file.ID == id).FirstOrDefault();

            if (fileEntity != null)
            {
                string fileName = $"{fileEntity.FILE_NAME}"; // You may need to retrieve the actual file name from the entity
                byte[] blobData = fileEntity.FILE_BLOB; // Replace "BlobColumn" with your actual property name

                return File(blobData, $"application/image", fileName);
            }
            else
            {
                return NotFound(); // Or any other appropriate status code
            }
        }
        catch (Exception ex)
        {
            _status = false;
            _code = 500;
            _message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : "";
            _error = ex.InnerException != null ? ex.InnerException.Message : "Not Inner Exception";
            return StatusCode(_code, new { status = _status, message = _message, error = _error, results = returnData });
        }
    }
}