using IWCRM.API.Data;
using IWCRM.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder( args );

// Add services
builder.Services.AddCors();
builder.Services.AddResponseCompression( options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[] { "application/json" } );
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[] { "image/jpeg", "image/png", "application/font-woff2", "image/svg+xml" } );
    options.EnableForHttps = true;
} );
// Add jwt Autenticator

var key = Encoding.ASCII.GetBytes( Settings.Secret );
builder.Services.AddAuthentication( x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer( x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey( key ),
        ValidateIssuer = false,
        ValidateAudience = false
    };
} );

// === Context
string connectionString = string.Empty;
connectionString = builder.Configuration.GetConnectionString( "DefaultConnection" );

builder.Services.AddDbContext<DataContext>( opt => opt.UseSqlite( connectionString ) );
builder.Services.AddScoped<DataContext, DataContext>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( c =>
{ 
    c.SwaggerDoc( "v1", new OpenApiInfo
    {
        Title = "IWCRM - Infowest CRM Api"
        ,
        Description = "Controle de Contatos, User : Admin Password : !Demo123"
        ,
        Version = "1.0.0 " ,
    } );
    c.CustomSchemaIds( ( type ) => type.ToString()
        .Replace( "[", "_" )
        .Replace( "]", "_" )
    .Replace( ",", "-" )
        .Replace( "`", "_" ) );
    c.ResolveConflictingActions( apiDescriptions => apiDescriptions.First() );

    var security = new Dictionary<string, IEnumerable<string>>
    {
        {"Bearer", new string[] { } }
    };

    c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    } );

    c.AddSecurityRequirement( new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    } );

} );
 
var app = builder.Build();

//if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.

app.UseSwaggerUI( c =>
{
    c.SwaggerEndpoint( "/swagger/v1/swagger.json", "CRM API V1" );
    c.DocumentTitle = "IW CRM";
} );

app.UseRouting();

// global cors policy
app.UseCors( x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader() );

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();
app.UseStaticFiles();

app.UseEndpoints( endpoints =>
{
    endpoints.MapControllers();
} );

app.Run();
