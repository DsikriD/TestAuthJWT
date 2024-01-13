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
        options.RequireHttpsMetadata = false; // ��������� ������������ ����� ��������� �� HTTP
                                              // � �� �� HTTPS
                                              // �� �������� ������� ����� ��������� true
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // ����� �� ������������ �������� ��� �������� ������ 
            ValidateIssuer = true,
            ValidIssuer =  authOptionsConfiguration.Issuer,

            //����� �� ������������ ����������� ������ 
            ValidateAudience =true,
            ValidAudience = authOptionsConfiguration.Audience,

            // ����� �� ������������ ����� ������������ ������ 
            ValidateLifetime = true,

            // ��������� ����� ������������
            IssuerSigningKey = authOptionsConfiguration.GetSymmetricSecurityKey(),//HS256
            //����� �� ������������ �������� ���� ������������ 
            ValidateIssuerSigningKey = true

        };

    });
//

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin() // ����������� ������� ������ � ������������ �������
            .AllowAnyMethod()       //  ����������� ������� ������ ������������� ����
            .AllowAnyHeader();     //   ����������� ������� � ������ �����������
        });
});

builder.Services.AddSingleton(new ProductStore());

var app = builder.Build();

app.UseRouting();// ������������� �������� � ���������� ��������� 
app.UseCors();//    ��������� ������������ Cors

// ������� �����, ������ ������ ����������� ����� Endpoint
app.UseAuthentication();// ������������
app.UseAuthorization();//  �����������
//

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

app.Run();
