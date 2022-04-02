using Newtonsoft.Json;

namespace UNPChecker.Models;

public class Email
{
    public string emailAddress { get; set; }
    public string password { get; set; }

    public static Email getAdminEmail()
    {
        var r = File.ReadAllText("email.json");
        return JsonConvert.DeserializeObject<Email>(r);
    }

}