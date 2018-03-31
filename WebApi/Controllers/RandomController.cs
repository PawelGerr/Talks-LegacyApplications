using System;
using System.Threading.Tasks;
using MainProcess;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	public class RandomController : Controller
	{
		private readonly RandomClient _random;

		public RandomController(RandomClient randomClient)
		{
			_random = randomClient ?? throw new ArgumentNullException(nameof(randomClient));
		}

		[HttpGet("next")]
		public async Task<IActionResult> NextAsync()
		{
			var value = await _random.NextAsync().ConfigureAwait(false);

			return Ok(value);
		}
	}
}
