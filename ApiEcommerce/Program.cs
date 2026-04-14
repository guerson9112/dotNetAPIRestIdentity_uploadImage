using System.Text;
using ApiEcommerce.Constants;
using ApiEcommerce.Models;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConectionString));

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddScoped<ICategoryRepository, CategoryRepositoy>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("SecretKey no esta configurada");
}
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; //validación https
            options.SaveToken = true; // guardar el token en el contexto de autenticación
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
)
;


builder.Services.AddControllers(option =>
    {
        option.CacheProfiles.Add(CacheProfiles.Defaul10, CacheProfiles.Profile10);
        option.CacheProfiles.Add(CacheProfiles.Defaul20, CacheProfiles.Profile20);

    }
);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// 1. Agregar SwaggerGen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      Description = "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                    "Ingresa la palabra a continuación el token generado en login.\n\r\n\r" +
                    "Ejemplo: \"12345abcdef\"",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          },
          Scheme = "oauth2",
          Name = "Bearer",
          In = ParameterLocation.Header
        },
        new List<string>()
      }
    });

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Ecommerce",
        Description = "API para gestionar productos y usuarios",
        TermsOfService = new Uri("http://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "DevTalles",
            Url = new Uri("https://devtalles.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia de uso",
            Url = new Uri("http://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "API Ecommerce V2",
        Description = "API para gestionar productos y usuarios",
        TermsOfService = new Uri("http://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "DevTalles",
            Url = new Uri("https://devtalles.com")
        },
        License = new OpenApiLicense
        {
            Name = "Licencia de uso",
            Url = new Uri("http://example.com/license")
        }
    });
});

var apiVersionBuilder = builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true; //para que los clientes sepan qué versiones están disponibles
    //option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version")); // ?api-version
});
apiVersionBuilder.AddApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVV"; //v1,v2,v3...
    option.SubstituteApiVersionInUrl = true; //api/v{version}/products 
}
);

builder.Services.AddCors(options =>
    {
        options.AddPolicy(PolicyNames.AllowSpecificOrigin,
            builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }
        );
    }
);

//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();

    // 2. Habilitar middleware para servir el JSON de Swagger
    app.UseSwagger();
    // 3. Habilitar middleware para servir la interfaz UI
    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    } );
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin);

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
