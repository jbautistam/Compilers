using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibScriptsSample
{
	/// <summary>
	///		Procesador de scripts
	/// </summary>
	public class ScriptManager
	{
		public ScriptManager(Interfaces.ILogger logger, Interfaces.IConsoleOutput console)
		{
			Context = new Manager.JobContextModel(this, logger, console);
		}

		/// <summary>
		///		Procesa la generación del informe
		/// </summary>
		public void ExecuteScript(string fileName, Dictionary<string, object> parameters)
		{
				// Procesa el archivo
				if (!ValidateData(fileName))
					Context.WriteError("Undefined data");
				else
				{
					Manager.ScriptProcessor processor = new Manager.ScriptProcessor(Context);

						// Log
						Context.WriteDebug($"Start process script {fileName}");
						// Procesa el script
						processor.Process(fileName, parameters);
						// Log
						Context.WriteDebug($"End process script {fileName}");
				}
	}

		/// <summary>
		///		Comprueba que los datos son correctos
		/// </summary>
		private bool ValidateData(string fileName)
		{
			bool validated = false;

				// Comprueba si los datos son correctos
				if (string.IsNullOrWhiteSpace(fileName))
					Context.WriteError("Script file name undefined");
				else if (!System.IO.File.Exists(fileName))
					Context.WriteError($"Can't find the script file {fileName}");
				else
					validated = true;
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Contexto de ejecución de script
		/// </summary>
		public Manager.JobContextModel Context { get; }
	}
}
