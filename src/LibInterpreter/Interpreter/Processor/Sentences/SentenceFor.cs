using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia de ejecución de un bucle 
	/// </summary>
	public class SentenceFor : SentenceBase
	{
		/// <summary>
		///		Nombre de variable
		/// </summary>
		public string Variable { get; set; }

		/// <summary>
		///		Expresión para el valor de inicio
		/// </summary>
		public string StartExpression { get; set; }

		/// <summary>
		///		Expresión para el valor de fin
		/// </summary>
		public string EndExpression { get; set; }

		/// <summary>
		///		Expresión para el valor del paso
		/// </summary>
		public string StepExpression { get; set; }

		/// <summary>
		///		Sentencias a ejecutar
		/// </summary>
		public SentenceCollection Sentences { get; } = new SentenceCollection();
	}
}
