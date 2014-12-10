using System.IO;
using System.Threading.Tasks;

namespace FolderSizeScanner.Document
{
	static class DocumentHelper
	{
		public static Task<IDocument> LoadDocumentAsync(string documentPath)
		{
			Task<IDocument> task;
			using (var reader = new DocumentReader(
				new FileStream(documentPath, FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				task = reader.ReadDocumentAsync();
			}

			return task;
		}

		public static Task SaveDocumentAsync(string documentPath, IDocument document)
		{
			Task task;
			using (var writer = new DocumentWriter(
				new FileStream(documentPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write)))
			{
				task = writer.WriteDocumentAsync(document);
			}

			return task;
		}
	}
}
