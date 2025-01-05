using System.Text;
using ArtcilesServer.Data;
using ArtcilesServer.Mapper;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using ArtcilesServer.Services;
using ArticlesServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DbConn>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string DefaultConnection not found.")));
builder.Services.AddSwaggerGen(c =>

{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "jwt api",
        Description = "api auth",
        Contact = new OpenApiContact
        {
            Name = "bilel",
            Email = "billel.boulahia456@gmail.com",

        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()

    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "jwt auth"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement

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
        new string[] {}

        }


    });

});
builder.Services.AddHealthChecks();

builder.Services.AddScoped<GenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<GenericRepository<Report>, GenericRepository<Report>>();
builder.Services.AddScoped<GenericRepository<Comment>, GenericRepository<Comment>>();
builder.Services.AddScoped<GenericRepository<Article>, GenericRepository<Article>>();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<CommentRepo>();
builder.Services.AddScoped<ReportRepo>();
builder.Services.AddScoped<ArticleRepo>();
builder.Services.AddScoped<HashPassword>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>

{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience=true,
        ValidateLifetime= true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuerSigningKey =true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogError($"Authentication failed: {context.Exception}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully");
            return Task.CompletedTask;
        }
    };
});




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();

//////only use when seeding or migrating 

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    try
//    {
//        var context = services.GetRequiredService<DbConn>();
//        //context.Database.Migrate();
//       // SeedData.Initialize(context);
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "An error occurred while seeding the database.");
//    }
//}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseJwtDebugMiddleware();
}

app.UseCors("AllowAll");



app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();







app.MapControllers();

app.Run();
