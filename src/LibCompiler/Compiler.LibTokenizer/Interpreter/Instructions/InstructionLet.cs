using System;

using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción para asignar un valor a una variable
	/// </summary>
	public class InstructionLet : InstructionBase
	{
		public InstructionLet(Token token) : base(token)
		{
			Variable = new ExpressionVariableIdentifier(token);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent + 1) + "--> Variable: " + Variable.GetDebugInfo();

				// Añade la información de las expresiones
				debug += Environment.NewLine + GetIndent(indent + 1) + "--> Expresiones: " + Expressions.GetDebugInfo();
				debug += Environment.NewLine + GetIndent(indent + 1) + "--> Expresiones RPN: " + ExpressionsRPN.GetDebugInfo();
				// Devuelve la información de depuración
				return debug;
		}

		/// <summary>
		///		Variable a la que se asigna el valor
		/// </summary>
		public ExpressionVariableIdentifier Variable { get; set; }

		/// <summary>
		///		Expresiones a asignar
		/// </summary>
		public ExpressionsCollection Expressions { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Expresiones a asignar en formato polaca inversa
		/// </summary>
		public ExpressionsCollection ExpressionsRPN { get; set; } = new ExpressionsCollection();
	}
}
