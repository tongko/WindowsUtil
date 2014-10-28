using System.IO;
using System.Reflection;

namespace SizeExplorer.Controls
{
	static class EmbededResources
	{
		private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

		public static Stream GetResourceStream(string resourcePath)
		{
			return Assembly.GetManifestResourceStream(resourcePath);
		}
	}
}
