using ContractDigitalSignature.Modules.Roles.Common.Extensions;
using ContractDigitalSignature.Modules.Roles.Data;

EnvFile.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

DbInitializer.Apply(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
var urls = app.Configuration["ASPNETCORE_URLS"] ?? string.Empty;
if (urls.Contains("https://", StringComparison.OrdinalIgnoreCase))
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.MapControllers();
app.Run();
