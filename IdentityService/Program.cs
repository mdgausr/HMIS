using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cfg = builder.Configuration;
var jwtKey = cfg["Jwt:Key"] ?? "ReplaceThisWithALongSecretKeyForDevOnly!";
var jwtIssuer = cfg["Jwt:Issuer"] ?? "hmis.local";

builder.Services.AddSingleton<IPasswordHasher<string>, PasswordHasher<string>>();
builder.Services.AddSingleton<IDbConnectionFactory>(sp => new NpgsqlConnectionFactory(cfg.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();

var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtIssuer,
        ValidateIssuer = true,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Simple DB factory & interfaces
public interface IDbConnectionFactory { System.Data.IDbConnection Create(); }
public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _conn;
    public NpgsqlConnectionFactory(string conn) => _conn = conn;
    public System.Data.IDbConnection Create() => new Npgsql.NpgsqlConnection(_conn);
}
