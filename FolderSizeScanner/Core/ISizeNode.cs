using System.Collections.Generic;

namespace FolderSizeScanner.Core
{
	public interface ISizeNode
	{
		#region Properties

		List<ISizeNode> Children { get; }

		bool IsFile { get; }

		string Name { get; }

		ISizeNode Parent { get; }

		string FullPath { get; }

		double Percentage { get; }

		ulong Size { get; }

		ulong SizeOnDisk { get; }

		#endregion


		#region Public Methods

		void AddChildNode(ISizeNode child);
		void AddSize(ulong size);
		void AddSizeOnDisk(ulong size);
		void SetIsFile();
		void SetParent(ISizeNode parent);

		#endregion
	}
}