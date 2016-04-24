using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MindSageWeb.Models;
using MindSageWeb.Services;
using MindSageWeb.Repositories;

namespace MindSageWeb
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var appConfig = Configuration.GetSection("AppConfigOptions").Get<AppConfigOptions>();
            services.Configure<AppConfigOptions>(option =>
            {
                option.GoogleClientSecret = appConfig.GoogleClientSecret;
                option.GoogleClinetId = appConfig.GoogleClinetId;
                option.ManagementPortalUrl = appConfig.ManagementPortalUrl;
                option.MindSageUrl = appConfig.MindSageUrl;
                option.PrimaryDBName = appConfig.PrimaryDBName;
                option.PaypalClientId = appConfig.PaypalClientId;
                option.PaypalClientSecret = appConfig.PaypalClientSecret;
                option.EmailSenderName = appConfig.EmailSenderName;
                option.SendGridUserName = appConfig.SendGridUserName;
                option.SendGridPassword = appConfig.SendGridPassword;
            });
            var databaseTable = Configuration.GetSection("DatabaseTableOptions").Get<DatabaseTableOptions>();
            services.Configure<DatabaseTableOptions>(option =>
            {
                option.ClassCalendars = databaseTable.ClassCalendars;
                option.ClassRooms = databaseTable.ClassRooms;
                option.Comments = databaseTable.Comments;
                option.CourseCatalogs = databaseTable.CourseCatalogs;
                option.FriendRequests = databaseTable.FriendRequests;
                option.LessonCatalogs = databaseTable.LessonCatalogs;
                option.LikeComments = databaseTable.LikeComments;
                option.LikeDiscussions = databaseTable.LikeDiscussions;
                option.LikeLessons = databaseTable.LikeLessons;
                option.Notifications = databaseTable.Notifications;
                option.Payments = databaseTable.Payments;
                option.StudentKeys = databaseTable.StudentKeys;
                option.UserActivities = databaseTable.UserActivities;
                option.UserProfiles = databaseTable.UserProfiles;
                option.Contracts = databaseTable.Contracts;
            });
            var errorMessages = Configuration.GetSection("ErrorMessageOptions").Get<ErrorMessageOptions>();
            services.Configure<ErrorMessageOptions>(option =>
            {
                option.CanNotConnectToTheDatabase = errorMessages.CanNotConnectToTheDatabase;
                option.CanNotChargeACreditCard = errorMessages.CanNotChargeACreditCard;
                option.CourseNotFound = errorMessages.CourseNotFound;
                option.UserProfileNotFound = errorMessages.UserProfileNotFound;
                option.CourseInformationIncorrect = errorMessages.CourseInformationIncorrect;
            });

            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonLetterOrDigit = false;
                option.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IClassCalendarRepository, ClassCalendarRepository>();
            services.AddTransient<IClassRoomRepository, ClassRoomRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<ICourseCatalogRepository, CourseCatalogRepository>();
            services.AddTransient<IDateTime, ServerDateTime>();
            services.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
            services.AddTransient<ILessonCatalogRepository, LessonCatalogRepository>();
            services.AddTransient<ILikeCommentRepository, LikeCommentRepository>();
            services.AddTransient<ILikeDiscussionRepository, LikeDiscussionRepository>();
            services.AddTransient<ILikeLessonRepository, LikeLessonRepository>();
            services.AddTransient<IStudentKeyRepository, StudentKeyRepository>();
            services.AddTransient<IUserActivityRepository, UserActivityRepository>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<Engines.IPayment, Engines.PaypalPayment>();
            services.AddTransient<Engines.IEmailSender, Engines.SendGridEmailSender>();

            services.AddTransient<Controllers.NotificationController, Controllers.NotificationController>();
            services.AddTransient<Controllers.MyCourseController, Controllers.MyCourseController>();
            services.AddTransient<Controllers.CourseController, Controllers.CourseController>();
            services.AddTransient<Controllers.ProfileController, Controllers.ProfileController>();

            var primaryDBConnectionString = Configuration["Data:DefaultConnection:PrimaryDBConnectionString"];
            services.AddTransient<MongoAccess.MongoUtil>(pvdr => new MongoAccess.MongoUtil(primaryDBConnectionString, appConfig.PrimaryDBName));

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            var appConfig = Configuration.GetSection("AppConfigOptions").Get<AppConfigOptions>();
            app.UseGoogleAuthentication(options =>
            {
                options.ClientId = appConfig.GoogleClinetId;
                options.ClientSecret = appConfig.GoogleClientSecret;
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
