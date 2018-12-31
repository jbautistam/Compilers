using System;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions
{
	/// <summary>
	///		Instrucción para incluir un archivo
	/// </summary>
	public class InstructionIncludeFile : LibTokenizer.Interpreter.Instructions.InstructionBase
	{
		public InstructionIncludeFile(TokenSmallCss token) : base(token)
		{
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + GetIndent(indent) + " --> Archivo: " + FileName;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }
	}
}
