using FolderSizeScanner.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FolderSizeScanner.Document
{
	public class DocumentVersionProvider
	{
		private const string VersionText = @"SizeScanner document ver0.0.1";

		public string GetVersion()
		{
			return VersionText;
		}
	}

	class DocumentReader : BinaryReader
	{
		public DocumentReader(Stream input)
			: base(input, Encoding.UTF8, false)
		{
		}

		/// <summary>
		/// Provide version text.
		/// </summary>
		/// <remarks>
		/// If you override this method, you must also override <see cref="DocumentWriter.VersionText"/>
		/// property so the Document are in sync.
		/// </remarks>
		protected virtual string VersionText { get { return new DocumentVersionProvider().GetVersion(); } }

		public Task<IDocument> ReadDocumentAsync()
		{
			return ReadDocumentImplAsync();
		}

		public void MoveToContentStart()
		{
			var byteCount = Read7BitEncodedInt();
			if (byteCount > 0)
				BaseStream.Seek(byteCount, SeekOrigin.Current);
		}

		protected virtual Task<IDocument> ReadDocumentImplAsync()
		{
			var task = Task.Run<IDocument>(() =>
			{
				EnsureVersion();
				//	Read computer name
				var computer = ReadString();
				//	Read result as of
				var asOf = ReadUtcDateTime().ToLocalTime();
				//	Read FSO count
				var fso = ReadInt64();
				//	Read drive size node.
				var array = ReadSizeNodeArray();

				return new FolderSizeScanner.Document.Document(computer, asOf, fso, array);
			});

			return task;
		}

		protected DateTime ReadUtcDateTime()
		{
			var ticks = ReadInt64();
			return new DateTime(ticks, DateTimeKind.Utc);
		}

		protected virtual void EnsureVersion()
		{
			//	Keep current position
			var pos = BaseStream.Position;
			BaseStream.Seek(0, SeekOrigin.Begin);
			var version = ReadString();
			if (!VersionText.Equals(version))
				throw new DocumentFormatException(string.Format("Document version incorrect. Expecting '{0}' but was '{1}'.",
					VersionText, version));

			if (pos > 0)
				BaseStream.Seek(pos, SeekOrigin.Begin);
		}

		protected virtual ISizeNode[] ReadSizeNodeArray()
		{
			var nCount = ReadInt32();
			var array = new ISizeNode[nCount];

			for (var i = 0; i < nCount; i++)
				array[i] = ReadSizeNode();

			return array;
		}

		protected virtual ISizeNode ReadSizeNode()
		{
			var fp = ReadString();
			var isFile = ReadBoolean();
			var name = ReadString();
			var size = ReadUInt64();
			var sod = ReadUInt64();
			var count = ReadInt32();
			var list = new List<ISizeNode>(count);
			for (var i = 0; i < count; i++)
				list.Add(ReadSizeNode());
			//, bool isFile, string name, ulong size, ulong sizeOnDisk, List<ISizeNode> children
			return new SizeNode(fp, isFile, name, size, sod, list);
		}
	}
}
