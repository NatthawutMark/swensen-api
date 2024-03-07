using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace swensen_api.Models;

public class ProductImageModel
{
    public string ID { get; set; }
    public string PRODUCCT_ID { get; set; }
    public string FILE_NAME { get; set; }
    [Column(TypeName = "BLOB")]
    public string FILE_EXT { get; set; }
    public byte[] FILE_BLOB { get; set; }
    public long FILE_SIZE { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public DateTime CREATED_AT { get; set; }
    public DateTime UPDATED_AT { get; set; }
}