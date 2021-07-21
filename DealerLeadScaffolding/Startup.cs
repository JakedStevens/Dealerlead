using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealerLead.Web
{
	public class Startup
	{
		static readonly DealerLeadDbContext _dbContext = new();

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(options =>
				{
					Configuration.Bind("AzureAD", options);
					options.Events ??= new OpenIdConnectEvents();
					options.Events.OnTokenValidated += OnTokenValidatedFunc;
				});

			services.AddControllersWithViews(options =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			});
			services.AddRazorPages()
				 .AddMicrosoftIdentityUI();
			services.AddTransient<DealerLeadDbContext>();
			services.AddScoped<UserService>();
		}

		private async Task OnTokenValidatedFunc(TokenValidatedContext context)
		{
			// Custom code here
			GetUserOid(context);
			await Task.CompletedTask.ConfigureAwait(false);
		}

		public async void Register(Guid userGuid)
		{
			var newUser = new DealerLeadUser() { AzureADId = userGuid };
			_dbContext.Add(newUser);
			await _dbContext.SaveChangesAsync();
		}

		public async void GetUserOid(TokenValidatedContext context)
		{
			var claimsList = context.Principal.Claims.ToList();
			var oidClaim = claimsList.FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			);
			Guid userGuid = Guid.Parse(oidClaim.Value);
			List<DealerLeadUser> userList = await _dbContext.DealerLeadUser.ToListAsync();
			bool userExists = userList.Any(user => user.AzureADId == userGuid);
			if (!userExists)
			{
				Register(userGuid);
			}

			// Eric's way
			//Guid? azureOIDToken = IdentityHelper.GetAzureOIDToken(context.Principal);
			//var user = _dbContext.DealerLeadUser.FirstOrDefault(x => x.AzureADId == azureOIDToken);

			//if (user == null)
			//{
			//	user = new DealerLeadUser { AzureADId = azureOIDToken.Value };
			//	_dbContext.Add(user);
			//	_dbContext.SaveChanges();
			//}
			//await Task.CompletedTask.ConfigureAwait(false);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}

