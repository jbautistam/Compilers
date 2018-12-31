using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Sentencia While
	/// </summary>
	internal class InstructionWhile : InstructionBase
	{
		internal InstructionWhile(Token token) : base(token)
		{
			Condition = new ExpressionsCollection();
			ConditionRPN = new ExpressionsCollection();
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent + 1) + " --> Condición " + Condition.GetDebugInfo();

				// Condiciones
				debug += Environment.NewLine + GetIndent(indent + 1) + " --> Condición RPN " + ConditionRPN.GetDebugInfo();
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Expresiones que forman la condición
		/// </summary>
		internal ExpressionsCollection Condition { get; set; }

		/// <summary>
		///		Condiciones en formato polaca inversa
		/// </summary>
		internal ExpressionsCollection ConditionRPN { get; set; }
	}
}
