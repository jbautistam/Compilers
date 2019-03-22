using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences
{
	/// <summary>
	///		Sentencia de retorno de una expresión para una función
	/// </summary>
	public class SentenceReturn : SentenceBase
	{
		/// <summary>
		///		Expresión
		/// </summary>
		public string Expression { get; set; }
	}
}
