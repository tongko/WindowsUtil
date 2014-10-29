using System;
using System.Linq;
using System.Management;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			var hdds = new ManagementObjectSearcher("SELECT * FROM Win32_DiskPartition").Get().Cast<ManagementObject>(); //	Obtain all HDD
			foreach (var hdd in hdds)
			{
				Console.WriteLine("======================================================================");
				foreach (var p in hdd.Properties)
				{
					if (hdd[p.Name] == null)
						Console.WriteLine("{0} : <NULL>", p.Name);
					else
						Console.WriteLine("{0} : {1}", p.Name, p.Value);
				}
			}
			//var ddi = new DiskDriveInfo();
			//ddi.PopulateInfo(null);

			Console.Write("Press any key to continue . . .");
			Console.ReadKey(true);
		}
	}
}
