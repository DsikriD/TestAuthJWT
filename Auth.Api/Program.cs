using Auth.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var authOptionsConfiguration = builder.Configuration.GetSection("Auth");
builder.Services.Configure<AuthOptions>(authOptionsConfiguration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


var app = builder.Build();

app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.Run();
