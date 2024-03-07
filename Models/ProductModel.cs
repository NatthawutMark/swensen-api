using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace swensen_api.Models;

public class ProductModel
{
    public string ID { get; set; }
    public string CATE_ID { get; set; }
    public string NAME { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal PRICE { get; set; }
    public int QTY { get; set; }
    public string STATUS_ID { get; set; }
    public int PROMOTION { get; set; }
    public int RECOMMEND { get; set; }
    public int NEWS { get; set; }

    // [Column(TypeName = "BLOB")]
    // public byte[] FILE_IMAGE { get; set; }
    // public string FILE_EXE { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public DateTime CREATED_AT { get; set; }
    public DateTime UPDATED_AT { get; set; }
}