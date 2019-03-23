using System;
using System.Collections.Generic;

using Bau.Libraries.LibScriptsSample;

namespace Bau.Applications.CompilerTestConsole
{
	static class Program
	{
		static void Main(string[] args)
		{
			string fileRoot = @"C:\Projects\Personal\Net\Libraries\Compilers\src\App\CompilerTestConsole\Data\MainTest.xml";
			ScriptManager manager = new ScriptManager(new Logger(Logger.LogItemType.Info), new ConsoleOutput());

				// Ejecuta los scritps
				foreach (Models.ScriptModel script in new Repository.ScriptRepository().Load(fileRoot))
				{
					// Log
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("Start process: " + script.FileName);
					// Ejecuta el script
					manager.ExecuteScript(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileRoot), script.FileName), script.Parameters);
					// Log
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("End process: " + script.FileName);
					Console.WriteLine(new string('-', 80));
					Console.WriteLine();
				}
				// Espera a que el usuario pulse una tecla
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.WriteLine("Press any key...");
				Console.ReadKey();
		}
	}
}
