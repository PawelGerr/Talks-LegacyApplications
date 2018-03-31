using System;

namespace Ipc.NamedPipes
{
	public interface IRequest
	{
		Guid Id { get; }
	}
}
