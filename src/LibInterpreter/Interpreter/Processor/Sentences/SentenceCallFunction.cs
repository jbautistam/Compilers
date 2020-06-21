using System;
using System.Collections.Generic;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia de llamada a una función
	/// </summary>
	public class SentenceCallFunction : SentenceBase
	{
		/// <summary>
		///		Nombre de la función a la que se llama
		/// </summary>
		public string Function { get; set; }

		/// <summary>
		///		Parámetros
		/// </summary>
		public List<string> ParameterExpressions { get; } = new List<string>();

		/// <summary>
		///		Variable en la que se retorna el resultado
		/// </summary>
		public string ResultVariable { get; set; }
	}
}
