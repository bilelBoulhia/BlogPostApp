using ArtcilesServer.Data;
using ArtcilesServer.Mapper;
using ArtcilesServer.Models;
using ArtcilesServer.Repo;
using ArtcilesServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DbConn>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string DefaultConnection not found.")));
builder.Services.AddSwaggerGen();
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
builder.Services.AddAutoMapper(typeof(MappingProfile));
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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");



app.MapControllers();

app.Run();
