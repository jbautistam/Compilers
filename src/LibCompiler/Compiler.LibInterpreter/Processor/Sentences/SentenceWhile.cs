using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia para un bucle While
	/// </summary>
	public class SentenceWhile : SentenceBase
	{
		/// <summary>
		///		Condición
		/// </summary>
		public string Condition { get; set; }

		/// <summary>
		///		Sentencias
		/// </summary>
		public SentenceCollection Sentences { get; } = new SentenceCollection();
	}
}
