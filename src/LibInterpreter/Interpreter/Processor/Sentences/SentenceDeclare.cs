using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia para declaración de una variable
	/// </summary>
	public class SentenceDeclare : SentenceBase
	{
		/// <summary>
		///		Nombre de variable
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Tipo de la variable
		/// </summary>
		public Context.Variables.VariableModel.VariableType Type { get; set; }

		/// <summary>
		///		Valor de la variable
		/// </summary>
		public object Value { get; set; }
	}
}
