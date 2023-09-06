using IWCRM.API.Data;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

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
//var key = Encoding.ASCII.GetBytes( Settings.Secret );
//builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
//    .AddJwtBearer( x =>
//    {
//        x.RequireHttpsMetadata = false;
//        x.SaveToken = true;
//        x.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey( key ),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    } );

// === Context

string connectionString = builder.Configuration.GetConnectionString( "DefaultConnection" ); 
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
        Description = "Api para testes de arquitetura Data Driven"
        ,
        Version = "1.0.0"
    } );
    c.CustomSchemaIds( ( type ) => type.ToString()
        .Replace( "[", "_" )
        .Replace( "]", "_" )
    .Replace( ",", "-" )
        .Replace( "`", "_" ) );
    c.ResolveConflictingActions( apiDescriptions => apiDescriptions.First() );
} );
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
