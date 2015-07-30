using System;
using System.IO;
using System.Reflection;
using FolderSizeScanner.Properties;

namespace FolderSizeScanner.Core
{
	static class EmbededResources
	{
		// Just to prevent beforefieldinit flag
		static EmbededResources() { }

		public static Stream GetResourceAsStream(string resourceName, Assembly asm)
		{
			if (asm == null) throw new ArgumentNullException("asm");
			if (string.IsNullOrWhiteSpace(resourceName))
				throw new ArgumentNullException("resourceName");

			var stream = asm.GetManifestResourceStream(resourceName);
			if (stream == null)
			{
				//                string message = "Could not find the resource: " + fileName +
				//                               ". Check that the script file has been set as EMBEDDED RESOURCE. Using assembly:" + assembly.Location;
				var message = string.Format(Resources.MsgResourceNotFound, resourceName, asm.Location);
				throw new ApplicationException(message);
			}

			return stream;
		}
	}
}
