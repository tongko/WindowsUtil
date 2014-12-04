namespace SizeExplorer.Model
{
	public struct DiskSizeInfoCache
	{
		public static readonly DiskSizeInfoCache Empty = new DiskSizeInfoCache
		{
			Root = null,
			SectorsPerCluster = uint.MinValue,
			BytesPerSector = uint.MinValue
		};

		public bool Equals(DiskSizeInfoCache other)
		{
			return string.Equals(Root, other.Root) && SectorsPerCluster == other.SectorsPerCluster && BytesPerSector == other.BytesPerSector;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Root != null ? Root.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int) SectorsPerCluster;
				hashCode = (hashCode * 397) ^ (int) BytesPerSector;
				return hashCode;
			}
		}

		public string Root;

		public uint SectorsPerCluster;

		public uint BytesPerSector;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is DiskSizeInfoCache && Equals((DiskSizeInfoCache) obj);
		}
	}
}
