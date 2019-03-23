using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibScriptsSample.Manager
{
	/// <summary>
	///		Contexto del trabajo
	/// </summary>
	public class JobContextModel
	{
		public JobContextModel(ScriptManager scriptManager, Interfaces.ILogger logger, Interfaces.IConsoleOutput consoleOutput)
		{
			ScriptManager = scriptManager;
			Logger = logger;
			ConsoleOutput = consoleOutput;
		}

		/// <summary>
		///		Escribe una advertencia en el log
		/// </summary>
		public void WriteWarning(string message)
		{
			Logger.WriteWarning(message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración en el log
		/// </summary>
		public void WriteDebug(string message)
		{
			Logger.WriteDebug(message);
		}

		/// <summary>
		///		Escribe un error en el log
		/// </summary>
		public void WriteError(string message)
		{
			// Escribe el error en el log
			Logger.WriteError(message);
			// Escribe el error en la consola
			WriteConsoleError(message);
			// Añade el error a la lista
			Errors.Add(message);
		}

		/// <summary>
		///		Escribe información en el log
		/// </summary>
		public void WriteInfo(string message)
		{
			Logger.WriteInfo(message);
		}

		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void WriteConsole(string message)
		{
			// Log
			WriteDebug(message);
			// Consola
			ConsoleOutput.Write(message);
		}

		/// <summary>
		///		Escribe un mensaje en la consola
		/// </summary>
		public void WriteConsoleError(string message)
		{
			ConsoleOutput.WriteError(message);
		}

		/// <summary>
		///		Manager de trabajos
		/// </summary>
		public ScriptManager ScriptManager { get; }

		/// <summary>
		///		Tratamiento de log
		/// </summary>
		public Interfaces.ILogger Logger;

		/// <summary>
		///		Interface para la consola de salida
		/// </summary>
		public Interfaces.IConsoleOutput ConsoleOutput { get; }

		/// <summary>
		///		Lista de errores
		/// </summary>
		public List<string> Errors { get; } = new List<string>();
	}
}
