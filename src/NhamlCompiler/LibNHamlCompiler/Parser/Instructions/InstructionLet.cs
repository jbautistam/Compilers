using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Instrucción para asignar un valor a una variable
	/// </summary>
	internal class InstructionLet : InstructionBase
	{
		internal InstructionLet(Token token) : base(token)
		{
			Variable = new ExpressionVariableIdentifier(token);
			Expressions = new ExpressionsCollection();
			ExpressionsRPN = new ExpressionsCollection();
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
		internal ExpressionVariableIdentifier Variable { get; set; }

		/// <summary>
		///		Expresiones a asignar
		/// </summary>
		internal ExpressionsCollection Expressions { get; set; }

		/// <summary>
		///		Expresiones a asignar en formato polaca inversa
		/// </summary>
		internal ExpressionsCollection ExpressionsRPN { get; set; }
	}
}
