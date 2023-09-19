using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace swensen_api.Models;

public class ProductModel
{
    public string ID { get; set; }
    public string NAME { get; set; }
    [Column(TypeName = "BLOB")]
    public byte[] FILE_IMAGE { get; set; }
    public int RECOMMEND { get; set; }
    public int IS_DELETE { get; set; }

    public int PROMOTION { get; set; }
    public int NEWS { get; set; }
    public string CATE_ID { get; set; }
}