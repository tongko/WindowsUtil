using System;

namespace Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			var ddi = new DiskDriveInfo();
			ddi.PopulateInfo(null);

			Console.Write("Press any key to continue . . .");
			Console.ReadKey(true);
		}
	}
}
