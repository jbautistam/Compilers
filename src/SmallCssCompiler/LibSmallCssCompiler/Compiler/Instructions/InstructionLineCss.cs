using System;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions
{
	/// <summary>
	///		Instrucción para una línea literal CSS
	/// </summary>
	public class InstructionLineCss : LibTokenizer.Interpreter.Instructions.InstructionBaseBlock
	{
		public InstructionLineCss(TokenSmallCss token) : base(token)
		{
			Tokens = new TokenSmallCssCollection();
			Tokens.Add(token);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine;

				// Añade la información de depuración del token
				debug = base.GetIndent(indent);
				foreach (TokenSmallCss token in Tokens)
					debug += token.GetDebugForInstructions() + " ";
				// Añade la información de depuración de las instrucciones
				debug += Sentences.GetDebugString(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Tokens hijo
		/// </summary>
		public TokenSmallCssCollection Tokens { get; private set; }
	}
}
