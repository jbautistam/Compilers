using System;

using Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences;

namespace Bau.Libraries.LibScriptsSample.Models
{
	/// <summary>
	///		Clase con los datos del programa
	/// </summary>
	internal class ProgramModel
	{
		/// <summary>
		///		Instrucciones del programa
		/// </summary>
		internal SentenceCollection Sentences { get; } = new SentenceCollection();
	}
}
