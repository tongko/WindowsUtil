using System.IO;
using System.Reflection;

namespace SizeExplorer.UI.Resources
{
	static class EmbededResources
	{
		private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

		public static Stream GetResourceStream(string name)
		{
			var resourcePath = typeof(EmbededResources).Namespace + "." + name;
			return Assembly.GetManifestResourceStream(resourcePath);
		}
	}
}
