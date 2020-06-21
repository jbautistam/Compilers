using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Clase con los datos de una excepción
	/// </summary>
	public class SentenceException : SentenceBase
	{
		/// <summary>
		///		Mensaje de error
		/// </summary>
		public string Message { get; set; }
	}
}
