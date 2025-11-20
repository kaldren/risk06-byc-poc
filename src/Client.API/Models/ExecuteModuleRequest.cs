namespace Client.API.Models;

public class ExecuteModuleRequest
{
    public string Module { get; set; }
    public string Endpoint { get; set; }
    public Dictionary<string, object> Params { get; set; } = new();
}
