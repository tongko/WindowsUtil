
using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSizeScanner.Core
{
	static class DocumentHelper
	{
		public static IDocument LoadDocument(string documentPath)
		{

		}

		public static void SaveDocument(string documentPath, IDocument document)
		{

		}

		private static IDocument ReadDocument(BinaryReader reader)
		{
			var dbl = reader.ReadInt64();
			var dt = new DateTime(dbl, DateTimeKind.Utc);

			var fsoCount = reader.ReadInt64();
			var nCount = reader.ReadInt32();
			var array = new ISizeNode[nCount];
			for (var i = 0; i < nCount; i++)
				array[i] = ReadSizeNode(reader);

			return new Document(dt, fsoCount, array);
		}

		private static ISizeNode ReadSizeNode(BinaryReader reader)
		{
			var fp = reader.ReadString();
			var isFile = reader.ReadBoolean();
			var name = reader.ReadString();
			var size = reader.ReadUInt64();
			var sod = reader.ReadUInt64();
			var count = reader.ReadInt32();
			var list = new List<ISizeNode>(count);
			for (var i = 0; i < count; i++)
				list.Add(ReadSizeNode(reader));
			//, bool isFile, string name, ulong size, ulong sizeOnDisk, List<ISizeNode> children
			return new SizeNode(fp, isFile, name, size, sod, list);
		}

		private static void WriteDocument(BinaryWriter writer, IDocument document)
		{
			writer.Write(document.ResultAsOf.Ticks);
			writer.Write(document.FSOCount);
			var count = document.Nodes.Length;
			writer.Write(count);
			for (var i = 0; i < count; i++)
				WriteSizeNode(writer, document.Nodes[i]);
		}

		private static void WriteSizeNode(BinaryWriter writer, ISizeNode node)
		{
			writer.Write(node.FullPath);
			writer.Write(node.IsFile);
			writer.Write(node.Name);
			writer.Write(node.Size);
			writer.Write(node.SizeOnDisk);

			var count = node.Children.Count;
			writer.Write(count);
			for (var i = 0; i < count; i++)
				WriteSizeNode(writer, node.Children[i]);
		}
	}
}
