namespace swensen_api.Models;

public class CategoryModel
{
    public string ID { get; set; }
    public string NAME_TH { get; set; }
    public string NAME_EN { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public DateTime CREATED_AT {get;set;}
    public DateTime UPDATED_AT {get;set;}

}