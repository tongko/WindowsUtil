using System;
using System.Diagnostics;
using SizeExplorer.Core;
using SizeExplorer.Model;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			const string FilePath = @"E:";
			//Win32Native.WIN32_FIND_DATA data;
			//var h = Win32Native.FindFirstFile(FilePath, out data);
			//h.Dispose();
			var sw = new Stopwatch();
			Console.WriteLine("Begining while construct.");
			var node = new SizeNode(FilePath, "");
			sw.Start();
			FileSizeHelper.Build(node);
			sw.Stop();
			Console.WriteLine("Time taken: {0}", sw.Elapsed);

			//sw.Reset();
			//Console.WriteLine("Begining recursive construct.");
			//node = new SizeNode(FilePath, "");
			//sw.Start();
			//FileSizeHelper.BuildTreeAtRoot(node, null, null);
			//sw.Stop();
			//Console.WriteLine("Time taken: {0}", sw.Elapsed);




			//var fi = new FileInfo(FilePath);

			//var node = new SizeNode(new DirectoryInfo(FilePath));
			//Console.WriteLine(@"Calculating . . .");
			//FileSizeHelper.BuildTree(node, null);
			//FileSizeHelper.CalculateSize(node);

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
