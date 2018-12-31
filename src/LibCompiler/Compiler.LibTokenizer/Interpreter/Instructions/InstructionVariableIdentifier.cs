using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción con un identificador de variable
	/// </summary>
	public class InstructionVariableIdentifier : InstructionBase
	{
		public InstructionVariableIdentifier(Lexical.Tokens.Token token) : base(token)
		{
			Variable = new ExpressionVariableIdentifier(token);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + GetIndent(indent + 1) + " --> Variable " + Variable.GetDebugInfo() +
				   Environment.NewLine + GetIndent(indent + 2) + " --> Valor " + Value.GetDebugInfo();
		}

		/// <summary>
		///		Expresión que identifica la variable
		/// </summary>
		public ExpressionVariableIdentifier Variable { get; set; }

		/// <summary>
		///		Expresiones con el valor
		/// </summary>
		public ExpressionsCollection Value { get; set; } = new ExpressionsCollection();
	}
}
