using System;

namespace Oragon.Contexts.ExceptionHandling
{
	[Flags]
	public enum ExceptionHandlingStrategy
	{
		BreakOnException,
		ContinueRunning
	}
}