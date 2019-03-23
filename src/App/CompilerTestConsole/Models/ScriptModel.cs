using System;
using System.Collections.Generic;

namespace Bau.Applications.CompilerTestConsole.Models
{
	/// <summary>
	///		Datos de ejecución de un script
	/// </summary>
	internal class ScriptModel
	{
		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Parámetros
		/// </summary>
		public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
	}
}
