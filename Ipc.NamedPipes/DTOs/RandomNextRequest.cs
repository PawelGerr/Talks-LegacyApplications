using System;

namespace Ipc.NamedPipes
{
	public class RandomNextRequest : IRequest
	{
		public Guid Id { get; set; }
	}
}
