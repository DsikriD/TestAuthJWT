using Auth.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Resource.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var authOptionsConfiguration = builder.Configuration.GetSection("Auth").Get<AuthOptions>();

//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
    {
        options.RequireHttpsMetadata = false; // Разрешает валидировать токен пришедший по HTTP
                                              // А не по HTTPS
                                              // на реальном сервере лучше выстовить true
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // Будет ли валидировать издатель при создании токена 
            ValidateIssuer = true,
            ValidIssuer =  authOptionsConfiguration.Issuer,

            //Будет ли валидировать потребитель токена 
            ValidateAudience =true,
            ValidAudience = authOptionsConfiguration.Audience,

            // Будет ли валидировать время существоания токена 
            ValidateLifetime = true,

            // Установка ключа безопасности
            IssuerSigningKey = authOptionsConfiguration.GetSymmetricSecurityKey(),//HS256
            //Буден ли валидировать заданный ключ безопасности 
            ValidateIssuerSigningKey = true

        };

    });
//

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin() // принимаются запросы только с определенных адресов
            .AllowAnyMethod()       //  принимаются запросы только определенного типа
            .AllowAnyHeader();     //   принимаются запросы с любыми заголовками
        });
});

builder.Services.AddSingleton(new ProductStore());

var app = builder.Build();

app.UseRouting();// Сопоставление запросов с конретными адрессами 
app.UseCors();//    Настройка конфигурации Cors

// Порядок важен, нельзя делать авторизацию после Endpoint
app.UseAuthentication();// Аунтефикация
app.UseAuthorization();//  Авторизация
//

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.Run();
