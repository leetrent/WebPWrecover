using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPWrecover.Data;
using WebPWrecover.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebPWrecover
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             // Database Connection Parameters
            String connectionString = buildConnectionString();
            
            // WRITE CONNECTION STRING TO THE CONSOLE
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("[Startup] Connection String: " + connectionString);
            Console.WriteLine("********************************************************************************");

            // NOW THAT WE HAVE OUR CONNECTION STRING, WE CAN ESTABLISH OUR DB CONTEXT
            services.AddDbContext<ApplicationDbContext>
            (
                options => options.UseMySQL(connectionString)
            );

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>()
            //     .AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }

        private String buildConnectionString()
        {
            Console.WriteLine("[Startup.buildConnectionString()] : BEGIN");

            String connectionString = null;
            try
            {
                connectionString = Environment.GetEnvironmentVariable("LOCAL_CONNECTION_STRING");
                if (connectionString == null)
                {
                    string vcapServices = System.Environment.GetEnvironmentVariable("VCAP_SERVICES");
                    if (vcapServices != null)
                    {
                        dynamic json = JsonConvert.DeserializeObject(vcapServices);
                        foreach (dynamic obj in json.Children())
                        {
                            dynamic credentials = (((JProperty)obj).Value[0] as dynamic).credentials;
                            if (credentials != null)
                            {
                                string host     = credentials.host;
                                string username = credentials.username;
                                string password = credentials.password;
                                string port     = credentials.port;
                                string db_name  = credentials.db_name;

                                connectionString = "Username=" + username + ";"
                                    + "Password=" + password + ";"
                                    + "Host=" + host + ";"
                                    + "Port=" + port + ";"
                                    + "Database=" + db_name + ";Pooling=true;";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in [Startup.buildConnectionString()]:");
                Console.WriteLine(e);
            }
            Console.WriteLine("[Startup.buildConnectionString()] : END");
            return connectionString;
        }
    }
}
