using System;
using MainProcess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
	class Program
	{
		static void Main(string[] args)
		{
			var webHost = new WebHostBuilder()
				.UseKestrel()
				.ConfigureServices(services =>
						{
							services.AddMvc();

							services.AddScoped<RandomClient>();
							services.AddScoped<ChildProcessPool>();
						})
				.Configure(builder => builder.UseMvc())
				.Build();

			webHost.Run();
		}
	}
}
