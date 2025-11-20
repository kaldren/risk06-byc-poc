using System.Text.Json.Serialization;

namespace Client.API.Models;

public class Module
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public List<ModuleEndpoint> Endpoints { get; set; } = new();
}

public class ModuleEndpoint
{
    public string? Name { get; set; }
    public Dictionary<string, string> Params { get; set; } = new();
    public ReturnType Returns { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReturnType
{
    Dict,
    List,
    String,
    Integer,
    Float,
    Boolean,
    None
}
