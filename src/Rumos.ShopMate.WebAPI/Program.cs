using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rumos.ShopMate.IoC;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.WebAPI.Security;

var builder = WebApplication.CreateBuilder(args); // server

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddShopMateServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserResolverService, UserResolverService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var keyText = builder.Configuration["JwtSettings:Key"] ?? throw new ArgumentException("Missing configuration key");
var keyBytes = Convert.FromBase64String(keyText);

if (keyBytes.Length < 32)
{
    throw new InvalidOperationException("The JWT key needs to be at least 32 random bytes");
}
if (string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Issuer"]) ||
    string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Audience"]) ||
    builder.Configuration.GetValue<int>("JwtSettings:ExpirationInMinutes") <= 0)
{
    throw new InvalidOperationException("Complete the JwtSettings configuration");
}


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = "name",
            RoleClaimType = "role"
        };

        options.Events = new JwtBearerEvents()
        {
            OnTokenValidated = context =>
            {
                var subject = context.Principal?.FindFirst("sub")?.Value;

                if (!int.TryParse(subject, out var id) || id <= 0)
                {
                    context.Fail("A valid user ID is required");
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();


const string angularDevelopment = "AngularDevelopment";

builder.Services.AddCors(options =>
{
    options.AddPolicy(angularDevelopment, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors(angularDevelopment);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
