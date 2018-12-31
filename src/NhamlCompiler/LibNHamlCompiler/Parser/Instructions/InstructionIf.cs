using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Sentencia if
	/// </summary>
	internal class InstructionIf : InstructionBase
	{
		internal InstructionIf(Token token) : base(token)
		{
			Condition = new ExpressionsCollection();
			ConditionRPN = new ExpressionsCollection();
			InstructionsElse = new InstructionsBaseCollection();
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent + 1) + " --> Condición " + Condition.GetDebugInfo();

				// Condiciones
				debug += Environment.NewLine + GetIndent(indent + 1) + " --> Condición RPN " + ConditionRPN.GetDebugInfo();
				// Else
				debug += Environment.NewLine + GetIndent(indent);
				if (InstructionsElse == null || InstructionsElse.Count == 0)
					debug += "Sin sentencia else";
				else
					debug += "Else " + Environment.NewLine + InstructionsElse.GetDebugString(indent + 1);
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

		/// <summary>
		///		Instrucciones de la parte else
		/// </summary>
		internal InstructionsBaseCollection InstructionsElse { get; set; }
	}
}
