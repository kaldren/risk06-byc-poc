using Client.API.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add server to OpenAPI documentation
builder.Services.AddSwaggerGen(c =>
{
    c.AddServer(new OpenApiServer
    {
        Url = // Get host dynamically from request
            builder.Environment.IsDevelopment()
                ? "https://localhost:5001"
                : "https://api.yourdomain.com",
        Description = "Local Development Server"
    });
});

builder.Services.AddScoped<IExecuteModuleService, ExecuteModuleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
{
    var serverUrl = $"{httpReq.Scheme}://{httpReq.Host.Value}";
    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
}));
app.UseSwaggerUI();

app.Run();
