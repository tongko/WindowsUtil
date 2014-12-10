using FolderSizeScanner.Core;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FolderSizeScanner.Document
{
	class DocumentWriter : BinaryWriter
	{
		public DocumentWriter(Stream output)
			: base(output, Encoding.UTF8, false)
		{
		}

		/// <summary>
		/// Provide version text.
		/// </summary>
		/// <remarks>
		/// If you override this method, you must also override <see cref="DocumentReader.VersionText"/>
		/// property so the Document are in sync.
		/// </remarks>
		protected virtual string VersionText { get { return new DocumentVersionProvider().GetVersion(); } }

		public Task WriteDocumentAsync(IDocument document)
		{
			return WriteDocumentImplAsync(document);
		}

		public void Write(DateTime localDateTime)
		{
			var dt = localDateTime;
			if (dt.Kind == DateTimeKind.Local)
				dt = dt.ToUniversalTime();

			Write(dt.Ticks);
		}

		protected virtual Task WriteDocumentImplAsync(IDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");

			var task = Task.Run(() =>
			{
				// Write VersionText.
				Write(VersionText);
				//	Write computer name.
				Write(document.ComputerName);
				//	Write result as of.
				Write(document.ResultAsOf);
				//	Write fso.
				Write(document.FsoCount);
				//	Write size nodes array.
				Write(document.Nodes);
			});

			return task;
		}

		protected virtual void Write(ISizeNode[] nodes)
		{
			if (nodes == null)
			{
				Write(0);
				return;
			}

			var len = nodes.Length;
			Write(len);
			for (var i = 0; i < len; i++)
				Write(nodes[i]);
		}

		protected virtual void Write(ISizeNode node)
		{
			//	Write full path.
			Write(node.FullPath);
			//	Write IsFile
			Write(node.IsFile);
			//	Write name.
			Write(node.Name);
			//	Write size.
			Write(node.Size);
			//	Write size on disk
			Write(node.SizeOnDisk);
			//	Write children
			var count = node.Children.Count;
			Write(count);
			for (var i = 0; i < count; i++)
				Write(node.Children[i]);
		}
	}
}
