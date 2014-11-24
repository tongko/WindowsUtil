using SizeExplorer.Core;
using System;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(60 / 64 + 1) ;

			//var rp = new ReparsePoint(@"D:\Projects");
			//Console.WriteLine("Target: {0}", rp.Target);
			//			var hdds = new ManagementObjectSearcher(
			//				@"ASSOCIATORS OF {Win32_Directory.Name='D:\'} 
			//				  WHERE AssocClass = Win32_Subdirectory 
			//					ResultRole = PartComponent").Get().Cast<ManagementObject>(); //	Obtain all HDD
			//			foreach (var hdd in hdds)
			//			{
			//				Console.WriteLine("======================================================================");
			//				foreach (var p in hdd.Properties)
			//				{
			//					if (hdd[p.Name] == null)
			//						Console.WriteLine("{0} : <NULL>", p.Name);
			//					else
			//						Console.WriteLine("{0} : {1}", p.Name, p.Value);
			//				}

			//				Console.ReadKey(true);
			//			}
			//var ddi = new DiskDriveInfo();
			//ddi.PopulateInfo(null);

			Console.Write("Press any key to continue . . .");
			Console.ReadKey(true);
		}
	}
}
