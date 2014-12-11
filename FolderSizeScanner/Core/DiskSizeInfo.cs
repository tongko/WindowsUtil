namespace FolderSizeScanner.Core
{
	internal struct DiskSizeInfo
	{
		public static readonly DiskSizeInfo Empty = new DiskSizeInfo(null, uint.MinValue, uint.MinValue);
		public readonly uint BytesPerSector;
		public readonly string Root;
		public readonly uint SectorsPerCluster;

		#region Constructors

		public DiskSizeInfo(string root, uint sectPerClust, uint bytPerSect)
		{
			Root = root;
			SectorsPerCluster = sectPerClust;
			BytesPerSector = bytPerSect;
		}

		#endregion

		#region Override Methods

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is DiskSizeInfo && Equals((DiskSizeInfo)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Root != null ? Root.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)SectorsPerCluster;
				hashCode = (hashCode * 397) ^ (int)BytesPerSector;
				return hashCode;
			}
		}

		#endregion

		#region Public Methods

		public bool Equals(DiskSizeInfo other)
		{
			return string.Equals(Root, other.Root) && SectorsPerCluster == other.SectorsPerCluster &&
				   BytesPerSector == other.BytesPerSector;
		}

		#endregion
	}
}