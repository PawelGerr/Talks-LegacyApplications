using System;

namespace Ipc.NamedPipes
{
	public class RandomNextResponse : IResponse
	{
		public Guid RequestId { get; set; }
		public int Value { get; set; }

		public RandomNextResponse()
		{
		}

		public RandomNextResponse(Guid requestId, int value)
		{
			RequestId = requestId;
			Value = value;
		}
	}
}
