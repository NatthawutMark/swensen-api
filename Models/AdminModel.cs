namespace swensen_api.Models;

public class AdminModel
{
    public string ID { get; set; }
    public string FIRST_NAME { get; set; }
    public string LAST_NAME { get; set; }
    public string EMAIL { get; set; }
    public string USERNAME { get; set; }
    public string PASSWORD { get; set; }
    public string TEL { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public DateTime CREATED_AT {get;set;}
    public DateTime UPDATED_AT {get;set;}
}