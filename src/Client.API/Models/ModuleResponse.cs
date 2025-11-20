using System.Text.Json.Serialization;

namespace Client.API.Models;

public class ModuleResponse
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
    public List<ModuleEndpoint> Endpoints { get; set; } = new();
}