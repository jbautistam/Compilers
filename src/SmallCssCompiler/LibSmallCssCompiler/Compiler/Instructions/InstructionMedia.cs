using System;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions
{
	/// <summary>
	///		Instrucción para las líneas "@media"
	/// </summary>
	public class InstructionMedia : LibTokenizer.Interpreter.Instructions.InstructionBase
	{
		public InstructionMedia(TokenSmallCss token) : base(token)
		{
			Parameters = "";
			Line = new InstructionLineCss(token);
		}

		/// <summary>
		///		Información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug;

				// Imprime los parámetros
				debug = base.GetIndent(indent);
				debug += $"@media {Parameters}";
				// Imprime las líneas internas
				return debug + Environment.NewLine + Line.GetDebugString(indent + 1);
		}

		/// <summary>
		///		Parámetros de la línea @media
		/// </summary>
		public string Parameters { get; set; }

		/// <summary>
		///		Líneas internas a la instrucción @media
		/// </summary>
		public InstructionLineCss Line { get; set; }
	}
}
