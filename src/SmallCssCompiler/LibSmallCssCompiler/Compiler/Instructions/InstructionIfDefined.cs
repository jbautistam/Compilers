using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions
{
	/// <summary>
	///		Instrucción "ifDefined"
	/// </summary>
	public class InstructionIfDefined : LibTokenizer.Interpreter.Instructions.InstructionIf
	{ 
		// Variables privadas
		private string identifier;

		public InstructionIfDefined(TokenSmallCss token) : base(token) { }

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent) + "--> IfDefined: " + Identifier;

				// Muestra las instrucciones
				debug += SentencesIf.GetDebugString(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Nombre de variable
		/// </summary>
		public string Identifier
		{
			get { return identifier; }
			set
			{
				identifier = value;
				if (!identifier.IsEmpty() && !identifier.StartsWith(CompilerConstants.Variable))
					identifier = CompilerConstants.Variable + identifier;
			}
		}
	}
}
