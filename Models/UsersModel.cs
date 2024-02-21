namespace swensen_api.Models;

public class Users
{
    public string ID { get; set; }
    public string USERNAME { get; set; }
    public string EMAIL { get; set; }
    public string PASSWORD { get; set; }
    public string FIRST_NAME { get; set; }
    public string LAST_NAME { get; set; }
    public int IS_ACTIVE { get; set; }
    public int IS_DELETE { get; set; }
    public string TEL { get; set; }
    public string GENDER { get; set; }
    public DateTime? BIRTH_DATE { get; set; }
}