namespace swensen_api.Models;

public class Status
{
    public string ID { get; set; }
    public string GROUP_ID { get; set; }
    public string NAME { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public DateTime CREATED_AT {get;set;}
    public DateTime UPDATED_AT {get;set;}
}