using System;

namespace SizeExplorer.Core
{
	public delegate void ThreadExceptionHandler(object sender, Exception ex);

	public interface IHandleThreadException
	{
		ThreadExceptionHandler ThreadExceptionHandlerCallback { get; set; }
	}
}
