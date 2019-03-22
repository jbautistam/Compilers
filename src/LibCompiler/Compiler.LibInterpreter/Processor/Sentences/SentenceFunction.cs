using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia de declaración de una función
	/// </summary>
	public class SentenceFunction : SentenceBase
	{
		/// <summary>
		///		Nombre de la función
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Tipo del resultado (Unknown indica que no devuelve ningún resultado)
		/// </summary>
		public VariableModel.VariableType Type { get; set; }

		/// <summary>
		///		Argumentos de la función
		/// </summary>
		public List<VariableModel> Arguments { get; } = new List<VariableModel>();

		/// <summary>
		///		Contenido de la función
		/// </summary>
		public SentenceCollection Sentences { get; } = new SentenceCollection();
	}
}
