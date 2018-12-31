using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción para ejecutar un bucle foreach
	/// </summary>
	public class InstructionForEach : InstructionBaseBlock
	{
		public InstructionForEach(Token token) : base(token)
		{
			IndexVariable = new ExpressionVariableIdentifier(token);
			ListVariable = new ExpressionVariableIdentifier(token);
		}

		/// <summary>
		///		Información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + base.GetIndent(indent) +
					" Variable: " + IndexVariable.GetDebugInfo() + " List: " + ListVariable.GetDebugInfo() +
					Environment.NewLine + Sentences.GetDebugString(indent + 1);
		}

		/// <summary>
		///		Variable índice
		/// </summary>
		public ExpressionVariableIdentifier IndexVariable { get; set; }

		/// <summary>
		///		Lista de variables
		/// </summary>
		public ExpressionVariableIdentifier ListVariable { get; set; }
	}
}
