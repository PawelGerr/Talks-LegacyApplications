using System;
using System.Threading.Tasks;
using MainProcess;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	public class RandomController : Controller
	{
		private readonly RandomClient _randomClient;

		public RandomController(RandomClient randomClient)
		{
			_randomClient = randomClient ?? throw new ArgumentNullException(nameof(randomClient));
		}

		[HttpGet("next")]
		public async Task<IActionResult> NextAsync()
		{
			var value = await _randomClient.NextAsync().ConfigureAwait(false);

			Console.WriteLine($"[{DateTime.Now}] Next random number is {value}");

			return Ok(value);
		}
	}
}
