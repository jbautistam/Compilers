using System;

using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Sentencia if
	/// </summary>
	public class InstructionIf : InstructionBase
	{
		public InstructionIf(Token token) : base(token) {}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent + 1) + " --> Condición " + Condition.GetDebugInfo();

				// Condiciones
				debug += Environment.NewLine + GetIndent(indent + 1) + " --> Condición RPN " + ConditionRPN.GetDebugInfo();
				// If / else
				debug += Environment.NewLine + GetDebugInfo("if", indent, SentencesIf);
				debug += Environment.NewLine + GetDebugInfo("else", indent, SentencesElse);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Obtiene la cadena de depuración de las sentencias del if o el else
		/// </summary>
		private string GetDebugInfo(string strSentence, int indent, InstructionsBaseCollection instructions)
		{
			string debug = GetIndent(indent) + strSentence;

				// Añade el salto de línea
				debug += Environment.NewLine;
				// Añade las instrucciones
				if (instructions == null || instructions.Count == 0)
					debug += "Sin instrucciones";
				else
					debug += instructions.GetDebugString(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Expresiones que forman la condición
		/// </summary>
		public ExpressionsCollection Condition { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Condiciones en formato polaca inversa
		/// </summary>
		public ExpressionsCollection ConditionRPN { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Instrucciones de la parte IF
		/// </summary>
		public InstructionsBaseCollection SentencesIf { get; set; } = new InstructionsBaseCollection();

		/// <summary>
		///		Instrucciones de la parte else
		/// </summary>
		public InstructionsBaseCollection SentencesElse { get; set; } = new InstructionsBaseCollection();
	}
}
