using System;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Instrucción con un identificador de variable
	/// </summary>
	internal class InstructionVariableIdentifier : InstructionBase
	{
		internal InstructionVariableIdentifier(Tokens.Token token) : base(token)
		{
			Variable = new ExpressionVariableIdentifier(token);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + GetIndent(indent + 1) + " --> Variable " + Variable.GetDebugInfo();
		}

		/// <summary>
		///		Expresión que identifica la variable
		/// </summary>
		internal ExpressionVariableIdentifier Variable { get; set; }
	}
}
