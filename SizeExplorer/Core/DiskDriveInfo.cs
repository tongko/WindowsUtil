using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;

namespace SizeExplorer.Core
{
	internal delegate void StatusChangedHandler(object sender, InfoStatusChangedEventArgs e);

	internal enum InfoStatus
	{
		None,
		Completed,
		InProgress
	}

	internal class InfoStatusChangedEventArgs : EventArgs
	{
		public InfoStatus Status { get; set; }

		public InfoStatusChangedEventArgs(InfoStatus status)
		{
			Status = status;
		}
	}

	internal abstract class InfoBase
	{
		private static long _key = 0;
		private InfoStatus _status;

		protected InfoBase()
		{
			Key = GetNextKey().ToString(CultureInfo.CurrentCulture);
		}

		public event StatusChangedHandler InfoStatusChanged;

		public string Key { get; protected set; }

		public string Name { get; set; }

		public uint Index { get; set; }

		public InfoStatus Status
		{
			get { return _status; }
			set
			{
				var temp = _status;
				_status = value;
				if (temp != _status)
					OnStatusChanged(_status);
			}
		}

		public abstract void PopulateInfo(InfoBase parent);

		protected virtual void OnStatusChanged(InfoStatus status)
		{
			if (InfoStatusChanged != null)
				InfoStatusChanged(this, new InfoStatusChangedEventArgs(status));
		}

		protected static long GetNextKey()
		{
			return _key++;
		}
	}

	internal class DiskDriveInfo : InfoBase
	{
		private readonly List<DiskDrive> _disks;

		public DiskDriveInfo()
		{
			_disks = new List<DiskDrive>();
		}

		public IList<DiskDrive> DiskDrives { get { return _disks; } }

		public override void PopulateInfo(InfoBase parent)
		{
			Status = InfoStatus.InProgress;
			Index = 0;

			var hdds = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get().Cast<ManagementObject>(); //	Obtain all HDD
			foreach (var disk in hdds)
			{
				if (string.IsNullOrWhiteSpace(Name))
					Name = disk["SystemName"] as string;
				var di = new DiskDrive { Name = disk["Caption"] as string, Index = (uint)disk["Index"], Size = (ulong)disk["Size"] };
				_disks.Add(di);
				di.PopulateInfo(this);
			}

			Status = InfoStatus.Completed;
		}
	}

	internal class DiskDrive : InfoBase
	{
		private readonly List<Partition> _parts;

		public DiskDrive()
		{
			_parts = new List<Partition>();
		}

		public ulong Size { get; set; }

		public IList<Partition> Partitions { get { return _parts; } }

		public override void PopulateInfo(InfoBase parent)
		{
			Status = InfoStatus.InProgress;

			if (parent == null) return;

			var parts =
				new ManagementObjectSearcher("SELECT * FROM Win32_DiskPartition WHERE DiskIndex = " + parent.Index).Get()
					.Cast<ManagementObject>();
			foreach (var part in parts)
			{
				var p = new Partition
				{
					Bootable = (bool)part["Bootable"],
					DiskDrive = parent as DiskDrive,
					Index = (uint)part["Index"],
					Name = part["Caption"] as string,
					Size = (ulong)part["Size"],
					DeviceId = part["DeviceId"] as string
				};
				_parts.Add(p);
				p.PopulateInfo(this);
			}

			Status = InfoStatus.Completed;
		}
	}

	internal class Partition : InfoBase
	{
		private readonly List<LogicalDrive> _drives;

		public Partition()
		{
			_drives = new List<LogicalDrive>();
		}

		public ulong Size { get; set; }

		public bool Bootable { get; set; }

		public DiskDrive DiskDrive { get; set; }

		public string DeviceId { get; set; }

		public IList<LogicalDrive> Drives { get { return _drives; } }

		public override void PopulateInfo(InfoBase parent)
		{
			Status = InfoStatus.InProgress;

			var ldps = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition").Get()
				.Cast<ManagementObject>().Where(ldp => ldp["Antecedent"].ToString().Contains(DeviceId));
			foreach (var ldp in ldps)
			{
				var ldpDependent = ldp["Dependent"].ToString();
				var startIndex = ldpDependent.IndexOf("\"", StringComparison.InvariantCulture) + 1;

				var drive = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk WHERE DeviceId = \""
														 + ldpDependent.Substring(startIndex, 2) + "\"").Get()
					.Cast<ManagementObject>()
					.FirstOrDefault();
				if (drive == null) continue;

				var d = new LogicalDrive
				{
					Index = 0,
					Name = drive["Caption"] as string,
					Size = (ulong)drive["Size"]
				};

				_drives.Add(d);
			}

			Status = InfoStatus.Completed;
		}
	}

	internal class LogicalDrive : InfoBase
	{
		public ulong Size { get; set; }

		public override void PopulateInfo(InfoBase parent)
		{

		}
	}
}
