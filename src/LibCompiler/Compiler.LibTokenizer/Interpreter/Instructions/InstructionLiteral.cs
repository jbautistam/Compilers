using System;

using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción con los datos de un literal
	/// </summary>
	public class InstructionLiteral : InstructionBase
	{
		public InstructionLiteral(Token objToken) : base(objToken) { }
	}
}
