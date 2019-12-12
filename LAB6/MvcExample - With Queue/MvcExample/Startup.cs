using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcExample.Repository;
using MassTransit;
using MvcExample.Services;

namespace MvcExample
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

			var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
			{
				var host = sbc.Host(new Uri("amqp://wsdtxtjx:Iw-juKWR7DZu4FFWzx7fAC-88ZFxSYPS@hawk.rmq.cloudamqp.com/wsdtxtjx"), h =>
				{
					h.Username("wsdtxtjx");
					h.Password("Iw-juKWR7DZu4FFWzx7fAC-88ZFxSYPS");
				});
			});

			bus.Start();

			var repository = new ReservationRepository();
			var service = new ReservationService(bus, repository);
			
			services.AddSingleton<IReservationRepository>(repository);
			services.AddSingleton<IReservationService>(service);

			services.AddControllersWithViews();
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
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
						 name: "default",
						 pattern: "{controller=Reservation}/{action=Index}/{id?}");
			});
		}
	}
}