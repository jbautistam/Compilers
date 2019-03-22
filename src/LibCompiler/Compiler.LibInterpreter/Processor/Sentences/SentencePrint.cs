using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia para imprimir un mensaje
	/// </summary>
	public class SentencePrint : SentenceBase
	{
		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; set; }
	}
}
