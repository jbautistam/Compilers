using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción de fin de programa
	/// </summary>
	public class InstructionEndProgram : InstructionBase
	{
		public InstructionEndProgram(Lexical.Tokens.Token token) : base(token) { }
	}
}
