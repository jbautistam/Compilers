using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Sentencia While
	/// </summary>
	public class InstructionWhile : InstructionBaseBlock
	{
		public InstructionWhile(Token token) : base(token)
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
				// Instrucciones
				debug += Environment.NewLine + Sentences.GetDebugString(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Expresiones que forman la condición
		/// </summary>
		public ExpressionsCollection Condition { get; set; }

		/// <summary>
		///		Condiciones en formato polaca inversa
		/// </summary>
		public ExpressionsCollection ConditionRPN { get; set; }
	}
}
