using Microsoft.Win32.SafeHandles;
using System.Security.Permissions;

namespace FolderSizeScanner.Core
{
	/// <summary>
	/// Wraps a FindFirstFile handle.
	/// </summary>
	sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeFindHandle"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFindHandle()
			: base(true)
		{
		}

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the 
		/// event of a catastrophic failure, false. In this case, it 
		/// generates a releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			return Win32.FindClose(handle);
		}
	}
}
