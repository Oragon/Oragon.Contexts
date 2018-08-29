using System;

namespace Oragon.Contexts.ExceptionHandling
{
	[Flags]
	public enum ExceptionHandlingStrategy
	{
        None,
		BreakOnException,
		ContinueRunning
	}
}