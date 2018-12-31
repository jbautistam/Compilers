using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Instrucción para ejecutar un bucle foreach
	/// </summary>
	internal class InstructionForEach : InstructionBase
	{
		internal InstructionForEach(Token token) : base(token)
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
								  " Variable: " + IndexVariable.GetDebugInfo() + " List: " + ListVariable.GetDebugInfo();
		}

		/// <summary>
		///		Variable índice
		/// </summary>
		internal ExpressionVariableIdentifier IndexVariable { get; set; }

		/// <summary>
		///		Lista de variables
		/// </summary>
		internal ExpressionVariableIdentifier ListVariable { get; set; }
	}
}
