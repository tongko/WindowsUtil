using Delimon.Win32.IO;
using SizeExplorer.Core;
using SizeExplorer.Model;
using System;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			const string FilePath = @"E:\Users\tliew\AppData\Local\Microsoft\Windows Store\Cache\0";
			//var fi = new FileInfo(FilePath);

			var node = new SizeNode(new DirectoryInfo(FilePath));
			Console.WriteLine(@"Calculating . . .");
			FileSizeHelper.BuildTree(node, null);
			FileSizeHelper.CalculateSize(node);

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

			//Console.WriteLine("C:\\ is {0} bytes", node.Size);
			Console.Write("Press any key to continue . . .");
			Console.ReadKey(true);
		}
	}
}
