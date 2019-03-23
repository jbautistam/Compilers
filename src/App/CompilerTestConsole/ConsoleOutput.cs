using System;

namespace Bau.Applications.CompilerTestConsole
{
	/// <summary>
	///		Consola de ejecución
	/// </summary>
	internal class ConsoleOutput : Libraries.LibScriptsSample.Interfaces.IConsoleOutput
	{
		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void Write(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message);
		}

		/// <summary>
		///		Escribe un mensaje de error en la consola
		/// </summary>
		public void WriteError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
		}
	}
}
