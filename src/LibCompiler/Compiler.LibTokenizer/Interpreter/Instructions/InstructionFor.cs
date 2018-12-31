using System;

using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción para ejecutar un bucle for
	/// </summary>
	public class InstructionFor : InstructionBaseBlock
	{
		public InstructionFor(Token token) : base(token)
		{
			StartInstruction = new InstructionLet(token);
			IncrementInstruction = new InstructionLet(token);
		}

		/// <summary>
		///		Información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + base.GetIndent(indent) +
						   Environment.NewLine + " StartInstruction: " + StartInstruction.GetDebugString(0) +
						   Environment.NewLine + " Condition: " + Condition.GetDebugInfo() +
						   Environment.NewLine + " ConditionRPN: " + ConditionRPN.GetDebugInfo() +
						   Environment.NewLine + " IncrementInstruction: " + IncrementInstruction.GetDebugString(0) +
						   Environment.NewLine + Sentences.GetDebugString(indent + 1);
		}

		/// <summary>
		///		Instrucción de arranque
		/// </summary>
		public InstructionLet StartInstruction { get; set; }

		/// <summary>
		///		Condición del fin del bucle
		/// </summary>
		public ExpressionsCollection Condition { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Condición del fin del bucle (RPN)
		/// </summary>
		public ExpressionsCollection ConditionRPN { get; set; } = new ExpressionsCollection();

		/// <summary>
		///		Instrucción de incremento
		/// </summary>
		public InstructionLet IncrementInstruction { get; set; }
	}
}
