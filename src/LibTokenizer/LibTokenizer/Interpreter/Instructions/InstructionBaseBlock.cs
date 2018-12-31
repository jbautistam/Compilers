using System;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción que define un bloque de instrucciones (es decir, instrucciones que contienen
	///	instrucciones como for, while, function...)
	/// </summary>
	public class InstructionBaseBlock : InstructionBase
	{
		public InstructionBaseBlock(Lexical.Tokens.Token token) : base(token)
		{
			Sentences = new InstructionsBaseCollection();
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Sentences.GetDebugString(indent);
		}

		/// <summary>
		///		Instrucciones
		/// </summary>
		public InstructionsBaseCollection Sentences { get; set; }
	}
}
