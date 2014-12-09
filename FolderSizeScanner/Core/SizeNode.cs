using System;
using System.Collections.Generic;
using System.IO;

namespace FolderSizeScanner.Core
{
	class SizeNode : ISizeNode
	{
		private static readonly string DirSeperator = new string(Path.DirectorySeparatorChar, 1);

		public SizeNode(string fullPath)
		{
			if (string.IsNullOrWhiteSpace(fullPath)) throw new ArgumentNullException("fullPath");

			FullPath = fullPath;
			Name = GetNameFromPath(fullPath);
			Children = new List<ISizeNode>();
		}

		public SizeNode(string fullPath, bool isFile, string name, ulong size, ulong sizeOnDisk, List<ISizeNode> children)
		{
			FullPath = fullPath;
			IsFile = isFile;
			Name = name;
			Size = size;
			SizeOnDisk = sizeOnDisk;
			Children = children;
			foreach (var child in Children)
			{
				child.SetParent(this);
			}
		}

		public List<ISizeNode> Children { get; private set; }

		public bool IsFile { get; private set; }

		public string Name { get; private set; }

		public ISizeNode Parent { get; private set; }

		public string FullPath { get; private set; }

		public double Percentage
		{
			get
			{
				return (Parent == null || Parent.SizeOnDisk == 0) ? 0d : SizeOnDisk / (double)Parent.SizeOnDisk * 100d;
			}
		}

		public ulong Size { get; private set; }

		public ulong SizeOnDisk { get; private set; }

		public void AddChildNode(ISizeNode child)
		{
			if (child == null) throw new ArgumentNullException("child");

			Children.Add(child);
			child.SetParent(this);
		}

		public void AddSize(ulong size)
		{
			if (size == 0) return;

			Size += size;
		}

		public void AddSizeOnDisk(ulong size)
		{
			if (size == 0) return;

			Size += size;
		}

		public void SetIsFile() { IsFile = true; }

		public void SetParent(ISizeNode parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			Parent = parent;
		}

		private static string GetNameFromPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return path;

			var length = path.EndsWith(DirSeperator) ? path.Length - 1 : path.Length;
			var index = length;
			while (--index >= 0)
			{
				var ch = path[index];
				if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar)
					return path.Substring(index + 1, length - index - 1);
			}

			return path;
		}

		#region ISizeNode Members

		List<ISizeNode> ISizeNode.Children { get { return Children; } }

		bool ISizeNode.IsFile { get { return IsFile; } }

		string ISizeNode.Name { get { return Name; } }

		ISizeNode ISizeNode.Parent { get { return Parent; } }

		string ISizeNode.FullPath { get { return FullPath; } }

		double ISizeNode.Percentage { get { return Percentage; } }

		ulong ISizeNode.Size { get { return Size; } }

		ulong ISizeNode.SizeOnDisk { get { return SizeOnDisk; } }

		void ISizeNode.AddChildNode(ISizeNode child) { AddChildNode(child); }

		void ISizeNode.AddSize(ulong size) { AddSize(size); }

		void ISizeNode.AddSizeOnDisk(ulong size) { AddSizeOnDisk(size); }

		void ISizeNode.SetIsFile() { SetIsFile(); }

		void ISizeNode.SetParent(ISizeNode parent) { SetParent(parent); }

		#endregion
	}
}
