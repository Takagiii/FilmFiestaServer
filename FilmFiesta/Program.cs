using FilmFiesta.Business;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.DataAccess.Repositories;
using FilmFiesta.Models.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutomapperProfiles));

// local DB
//builder.Services.AddEntityFrameworkSqlServer()
//                        .AddDbContext<FilmFiestaContext>(options => options.UseSqlServer("name=ConnectionStrings:FilmFiesta"));

// online DB
builder.Services.AddEntityFrameworkSqlServer()
                        .AddDbContext<FilmFiestaContext>(options => options.UseSqlServer("name=ConnectionStrings:OnlineDatabase"));

Microsoft.Extensions.Configuration.IConfigurationSection jwtSettings = builder.Configuration.GetSection("JWTSettings");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});

builder.Services.AddTransient<IMoviesRepository, MoviesRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<ISubscriptionsRepository, SubscriptionsRepository>();
builder.Services.AddTransient<IGenresRepository, GenresRepository>();
builder.Services.AddTransient<IMoviesGenresRepository, MoviesGenresRepository>();
builder.Services.AddTransient<IMoviesVotesRepository, MoviesVotesRepository>();

builder.Services.AddTransient<IMoviesBusiness, MoviesBusiness>();
builder.Services.AddTransient<IUsersBusiness, UsersBusiness>();
builder.Services.AddTransient<ISubscriptionsBusiness, SubscriptionsBusiness>();
builder.Services.AddTransient<IGenresBusiness, GenresBusiness>();
builder.Services.AddTransient<IMoviesGenresBusiness, MoviesGenresBusiness>();
builder.Services.AddTransient<IMoviesVotesBusiness, MoviesVotesBusiness>();

builder.Services.AddScoped<JwtHandler>();

//builder.Services.AddIdentityCore<AspNetUser>().AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "OwnOrigins",
        policy =>
        {
            _ = policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FilmFiestaServer",
        Description = "The server for FilmFiesta",
    });

    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlPath));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

_ = app.UseSwagger();
_ = app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers().RequireCors("OwnOrigins");


app.Run();

//A décommenter pour tester
//partial class Program
//{
//    static async Task Main(string[] args)
//    {
//        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
//        ILogger logger = loggerFactory.CreateLogger("Program");

//        MapperConfiguration config = new(cfg =>
//        {
//            cfg.AddProfile<AutomapperProfiles>();
//        });

//        IMapper mapper = config.CreateMapper();

//        AdvicesRepository adviceRepository = new(new natureguardserver.DataAccess.EfModels.PostgresContext(), logger, mapper);
//        AreasRepository areasRepository = new(new natureguardserver.DataAccess.EfModels.PostgresContext(), logger, mapper);
//        AreasVoteRepository areasVoteRepository = new(new natureguardserver.DataAccess.EfModels.PostgresContext(), logger, mapper);
//        ReportsRepository reportsRepository = new(new natureguardserver.DataAccess.EfModels.PostgresContext(), logger, mapper);
//        ReportsVoteRepository reportsVoteRepository = new(new natureguardserver.DataAccess.EfModels.PostgresContext(), logger, mapper);

//        var advice = new natureguardserver.Dbo.Advice()
//        {
//            Id = 12,
//            DescriptionAdvice = "Testupdate",
//            Level = 5,
//            AdviceType = AdviceType.test2,
//        };

//        adviceRepository.Update(advice);
//        //areasRepository.Delete(1);
//        //areasVoteRepository.Delete(1);
//        //reportsRepository.Delete(1);
//        //reportsVoteRepository.Delete(1);

//        //await adviceRepository.Insert(new natureguardserver.Dbo.Advice()
//        //{
//        //    DescriptionAdvice = "Test1",
//        //    Level = 1,
//        //    AdviceType = AdviceType.test1,
//        //});
//        //Advice advice = adviceRepository.Get(1);

//        //await areasRepository.Insert(new natureguardserver.Dbo.Area()
//        //{
//        //    Date = DateTime.UtcNow,
//        //    IdUser = 1,
//        //    Localisation = new Polygon(new LinearRing(new Coordinate[] {
//        //            new(0, 0),
//        //            new(0, 10),
//        //            new(10, 10),
//        //            new(10, 0),
//        //            new(0, 0)
//        //        })),
//        //    AreaType = AreaType.Forest
//        //});
//        //Area area = areasRepository.Get(1);

//        //await areasVoteRepository.Insert(new natureguardserver.Dbo.AreaVote()
//        //{
//        //    Date = DateTime.UtcNow,
//        //    IdArea = 1,
//        //    IdUser = 1
//        //});
//        //AreaVote areaVote = areasVoteRepository.Get(1);

//        //await reportsRepository.Insert(new Report()
//        //{
//        //    Localisation = new NpgsqlTypes.NpgsqlPoint(52.510, 58.50084),
//        //    Date = DateTime.UtcNow,
//        //    IdUser = 1,
//        //    ReportType = ReportType.Fire,
//        //});
//        //Report report = reportsRepository.Get(1);

//        //await reportsVoteRepository.Insert(new natureguardserver.Dbo.ReportVote()
//        //{
//        //    Date = DateTime.UtcNow,
//        //    IdReport = 1,
//        //    IdUser = 1
//        //});
//        //ReportVote reportVote = reportsVoteRepository.Get(1);
//    }
//}