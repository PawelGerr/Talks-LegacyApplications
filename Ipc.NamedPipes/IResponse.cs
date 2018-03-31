using System;

namespace Ipc.NamedPipes
{
	public interface IResponse
	{
		Guid RequestId { get; }
	}
}
