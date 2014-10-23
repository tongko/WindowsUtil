using System;
using System.Management;

namespace SizeExplorer.Model
{
	public struct Win32PhysicalMedia
	{
		public string Caption;
		public string Description;
		public DateTime InstallDate;
		public string Name;
		public string Status;
		public string CreationClassName;
		public string Manufacturer;
		public string Model;
		public string SKU;
		public string SerialNumber;
		public string Tag;
		public string Version;
		public string PartNumber;
		public string OtherIdentifyingInfo;
		public bool PoweredOn;
		public bool Removable;
		public bool Replaceable;
		public bool HotSwappable;
		public ulong Capacity;
		public ushort MediaType;
		public string MediaDescription;
		public bool WriteProtectOn;
		public bool CleanerMedia;

		public static Win32PhysicalMedia FromMedia(ManagementBaseObject media)
		{
			var m = new Win32PhysicalMedia();

			if (media["Caption"] == null)
				m.Caption = "[NULL]";
			else
				m.Caption = media["Caption"] as string;
			if (media["Description"] == null)
				m.Description = "[NULL]";
			else
				m.Description = media["Description"] as string;
			if (media["InstallDate"] == null)
				m.InstallDate = DateTime.MinValue;
			else
				m.InstallDate = (DateTime)media["InstallDate"];
			if (media["Name"] == null)
				m.Name = "[NULL]";
			else
				m.Name = media["Name"] as string;
			if (media["Status"] == null)
				m.Status = "[NULL]";
			else
				m.Status = media["Status"] as string;
			if (media["CreationClassName"] == null)
				m.CreationClassName = "[NULL]";
			else
				m.CreationClassName = media["CreationClassName"] as string;
			if (media["Manufacturer"] == null)
				m.Manufacturer = "[NULL]";
			else
				m.Manufacturer = media["Manufacturer"] as string;
			if (media["Model"] == null)
				m.Model = "[NULL]";
			else
				m.Model = media["Model"] as string;
			if (media["SKU"] == null)
				m.SKU = "[NULL]";
			else
				m.SKU = media["SKU"] as string;
			if (media["SerialNumber"] == null)
				m.SerialNumber = "[NULL]";
			else
				m.SerialNumber = media["SerialNumber"] as string;
			if (media["Tag"] == null)
				m.Tag = "[NULL]";
			else
				m.Tag = media["Tag"] as string;
			if (media["Version"] == null)
				m.Version = "[NULL]";
			else
				m.Version = media["Version"] as string;
			if (media["PartNumber"] == null)
				m.PartNumber = "[NULL]";
			else
				m.PartNumber = media["PartNumber"] as string;
			if (media["OtherIdentifyingInfo"] == null)
				m.OtherIdentifyingInfo = "[NULL]";
			else
				m.OtherIdentifyingInfo = media["OtherIdentifyingInfo"] as string;
			if (media["PoweredOn"] == null)
				m.PoweredOn = false;
			else
				m.PoweredOn = (bool)media["PoweredOn"];
			if (media["Removable"] == null)
				m.Removable = false;
			else
				m.Removable = (bool)media["Removable"];
			if (media["Replaceable"] == null)
				m.Replaceable = false;
			else
				m.Replaceable = (bool)media["Replaceable"];
			if (media["HotSwappable"] == null)
				m.HotSwappable = false;
			else
				m.HotSwappable = (bool)media["HotSwappable"];
			if (media["Capacity"] == null)
				m.Capacity = 0;
			else
				m.Capacity = (ulong)media["Capacity"];
			if (media["MediaType"] == null)
				m.MediaType = 0;
			else
				m.MediaType = (ushort)media["MediaType"];
			if (media["MediaDescription"] == null)
				m.MediaDescription = "[NULL]";
			else
				m.MediaDescription = media["MediaDescription"] as string;
			if (media["WriteProtectOn"] == null)
				m.WriteProtectOn = false;
			else
				m.WriteProtectOn = (bool)media["WriteProtectOn"];
			if (media["CleanerMedia"] == null)
				m.CleanerMedia = false;
			else
				m.CleanerMedia = (bool)media["CleanerMedia"];


			return m;
		}
	}
}