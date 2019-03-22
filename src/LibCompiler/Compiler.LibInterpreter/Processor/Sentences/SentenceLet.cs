using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia para ejecución de una expresión
	/// </summary>
	public class SentenceLet : SentenceBase
	{
		/// <summary>
		///		Tipo de la variable de salida: sólo es necesario cuando no está definida
		/// </summary>
		public Context.Variables.VariableModel.VariableType Type { get; set; }

		/// <summary>
		///		Nombre de variable
		/// </summary>
		public string Variable { get; set; }

		/// <summary>
		///		Expresión a ejecutar
		/// </summary>
		public string Expression { get; set; }
	}
}
