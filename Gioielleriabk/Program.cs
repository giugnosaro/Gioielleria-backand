using Gioielleriabk.Middleware;
using Gioielleriabk.Services;
using Gioielleriabk.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Legge la stringa di connessione da appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configura l'autenticazione JWT
var key = Encoding.ASCII.GetBytes("questa_e_una_chiave_segreta_per_jwt"); // Usa una chiave più sicura in produzione
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true
    };
});
 void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseMiddleware<RoleAuthorizationMiddleware>(); // Utilizza il middleware di autorizzazione

    // Altre configurazioni
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
// Aggiungi CORS per consentire richieste dal frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Registra il servizio UserService per l'iniezione delle dipendenze
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ProdottiService>();
builder.Services.AddScoped<OrderService>();

// Aggiungi Swagger e controller
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost4200");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
